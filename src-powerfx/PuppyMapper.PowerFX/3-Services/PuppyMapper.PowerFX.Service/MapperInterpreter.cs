using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LisOfInputs = 
    System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<(string Key, Microsoft.PowerFx.Types.FormulaValue Value)>>;

namespace PuppyMapper.PowerFX.Service;

public class MapperInterpreter
{
    private readonly IMappingDocument _doc;
    private readonly ImmutableDictionary<string, IMappingDocument> _childMappers;
    private readonly RecalcEngine _engine;

    static void OnUpdate(string name, FormulaValue newValue)
    {
        Console.Write($"{name}: ");
        if (newValue is ErrorValue errorValue)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + errorValue.Errors[0].Message);
            Console.ResetColor();
        }
        else
        {
            if (newValue is TableValue)
                Console.WriteLine("");
            //Console.WriteLine(PrintResult(newValue));
        }
    }
    public static Dictionary<string, object>  MapRecord(
        string inputJsonFile,
        MappingDocument doc)
    {
        Dictionary<string, object> result = new();
        RecalcEngine engine = new RecalcEngine();

        var input = FormulaValueJSON.FromJson(File.ReadAllText(inputJsonFile));
        engine.UpdateVariable("input", input);
        foreach (var kv in doc.InternalVars.Rules)
        {
            var newField = kv.Name;
            engine.SetFormula(kv.Name, kv.Formula, OnUpdate);

        }
        foreach (var kv in doc.MappingRules.Rules)
        {
            var value = engine.Eval(kv.Formula);

            result[kv.Name] = value.ToObject();
        }
        return result;

    }
    public static Dictionary<string, object> MapRecord(
        MappingDocument doc, IEnumerable<(string Key, FormulaValue Value)> inputs)
    {
        Dictionary<string, object> result = new();
        RecalcEngine engine = new RecalcEngine();
        foreach(var input in inputs)
        {
            engine.UpdateVariable(input.Key, input.Value);
        }
        foreach (var kv in doc.InternalVars.Rules)
        {
            var newField = kv.Name;
            engine.SetFormula(kv.Name, kv.Formula, OnUpdate);

        }
        foreach (var kv in doc.MappingRules.Rules)
        {
            var value = engine.Eval(kv.Formula);

            result[kv.Name] = value.ToObject();
        }
        return result;
    }

    public static RecordValue MapRecordAsFormulaValue(
        IMappingDocument doc, FormulaValue input, ImmutableDictionary<string, IMappingDocument> childMappers)
    {
        RecalcEngine engine = new RecalcEngine();
        engine.UpdateVariable("input", input);
        List<NamedValue> fields = new List<NamedValue>();
        foreach (var kv in doc.InternalVars.Rules)
        {
            var newField = kv.Name;
            engine.SetFormula(kv.Name, kv.Formula, OnUpdate);
        }
        foreach (var kv in doc.MappingRules.Rules)
        {
            var currFormula = kv.Formula;
            if (currFormula.StartsWith("Map "))
            {
                var mapCommandParts = currFormula.Split(" ");
                var paramToPass = engine.Eval(mapCommandParts[1]);
                var nameOfMapperToUse = mapCommandParts[3];
                var mapperToUse = childMappers[nameOfMapperToUse];
                var resultOfMap = MapRecordAsFormulaValue(mapperToUse, paramToPass, childMappers);
            }
            var value = engine.Eval(kv.Formula);

            fields.Add(new NamedValue(kv.Name, value));
        }
        return RecordValue.NewRecordFromFields(fields);
    }

    public MapperInterpreter(IMappingDocument doc, ImmutableDictionary<string, IMappingDocument> childMappers)
    {
        _doc = doc;
        _childMappers = childMappers;
        _engine = new RecalcEngine();
        //PowerFxIntegration.RegisterCustomFunction(_engine);
        //foreach (var input in _doc.MappingInputs)
        //{
        //    _engine.UpdateVariable(input.InputName, FormulaValue.NewBlank(FormulaType.UntypedObject));
        //}
        //foreach (var kv in doc.InternalVars.Rules)
        //{
        //    var newField = kv.Name;
        //    _engine.SetFormula(kv.Name, kv.Formula, OnUpdate);
        //}
    }

    public IEnumerable<Dictionary<string, object>> MapRecords(
        LisOfInputs rows)
    {
        return MapRecords(_doc, rows);
    }
    public IEnumerable<Dictionary<string, object>> MapRecords(IMappingDocument doc,
        LisOfInputs rows)
    {
        var firstRow = true;
        Dictionary<string, object>? prevResult = null;
        foreach(var row in rows)
        {
            Dictionary<string, object> result = new();
            PopulateInputsAsVariables(row, prevResult);
            if (firstRow)
            {
                AddVariables(doc);
                firstRow = false;
            }
            PopulateResult(doc, result);
            yield return result;
            prevResult = result;
        }

    }

    private void PopulateInputsAsVariables(IEnumerable<(string Key, FormulaValue Value)> row, Dictionary<string, object>? prevResult)
    {
        foreach (var input in row)
        {
            _engine.UpdateVariable(input.Key, input.Value);
        }

        if (prevResult == null) return;
        var fields = prevResult.Select(kv => new NamedValue(kv.Key, ConvertToFormulaValue(kv.Value)));
        var prevResultRecord = FormulaValue.NewRecordFromFields(fields);
        _engine.UpdateVariable("PREV_ROW", prevResultRecord);
    }

    private void AddVariables(IMappingDocument doc)
    {
        foreach (var kv in doc.InternalVars.Rules)
        {
            var newField = kv.Name;
            _engine.SetFormula(kv.Name, kv.Formula, OnUpdate);
        }
    }

    private void PopulateResult(IMappingDocument doc, Dictionary<string, object> result)
    {
        foreach (var kv in doc.MappingRules.Rules)
        {
            var currFormula = kv.Formula.Trim();
            if (currFormula.StartsWith("Map "))
            {
                var mapCommandParts = currFormula.Split(" ");
                var paramToPass = _engine.Eval(mapCommandParts[1]);
                var nameOfMapperToUse = mapCommandParts[2];
                var mapperToUse = _childMappers[nameOfMapperToUse];
                var resultOfMap = MapRecordAsFormulaValue(mapperToUse, paramToPass, _childMappers);
                result[kv.Name] = resultOfMap.ToObject();
            } 
            else
            {
                var value = _engine.Eval(kv.Formula);

                result[kv.Name] = value.ToObject();
            }
        }
    }

    public static FormulaValue ConvertToFormulaValue(object? value)
    {
        // preserve already-built FormulaValue
        if (value is FormulaValue fv)
            return fv;
    
        if (value is null)
            return FormulaValue.NewBlank(FormulaType.UntypedObject);
    
        if (value is bool b)
            return FormulaValue.New(b);
    
        if (value is string s)
            return FormulaValue.New(s);
    
        if (value is int i)
            return FormulaValue.New(i);
    
        if (value is long l)
            return FormulaValue.New(l);
    
        if (value is double d)
            return FormulaValue.New(d);
    
        if (value is float f)
            return FormulaValue.New(f);
    
        if (value is decimal dec)
            return FormulaValue.New(dec);
    
        if (value is Guid g)
            return FormulaValue.New(g);
    
        if (value is DateTime dt)
            return FormulaValue.New(dt);
    
        if (value is TimeSpan ts)
            return FormulaValue.New(ts);
    
        // If the input is already a dictionary of string -> object, convert into a RecordValue
        if (value is IDictionary<string, object?> dictObj)
        {
            var fields = dictObj.Select(kv => new NamedValue(kv.Key, ConvertToFormulaValue(kv.Value)));
            return RecordValue.NewRecordFromFields(fields);
        }
    
        // If the input is a dictionary of string -> FormulaValue, map directly
        if (value is IDictionary<string, FormulaValue> dictFv)
        {
            var fields = dictFv.Select(kv => new NamedValue(kv.Key, kv.Value));
            return RecordValue.NewRecordFromFields(fields);
        }
    
        // If the input is a sequence of key/value pairs (e.g. IEnumerable<KeyValuePair<string, object>>)
        if (value is IEnumerable<KeyValuePair<string, object?>> kvEnum)
        {
            var fields = kvEnum.Select(kv => new NamedValue(kv.Key, ConvertToFormulaValue(kv.Value)));
            return RecordValue.NewRecordFromFields(fields);
        }
    
        // Fallback: try enum as integer, else use string representation
        var t = value.GetType();
        if (t.IsEnum)
            return FormulaValue.New(Convert.ToInt32(value));
    
        return FormulaValue.New(value.ToString() ?? string.Empty);
    }
    

}
