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
    private readonly MappingDocument _doc;
    private readonly ImmutableDictionary<string, MappingDocument> _childMappers;
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
        MappingDocument doc, FormulaValue input, ImmutableDictionary<string, MappingDocument> childMappers)
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

    public MapperInterpreter(MappingDocument doc, ImmutableDictionary<string, MappingDocument> childMappers)
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
    public IEnumerable<Dictionary<string, object>> MapRecords(MappingDocument doc,
        LisOfInputs rows)
    {
        var firstRow = true;
        foreach(var row in rows)
        {
            Dictionary<string, object> result = new();
            foreach (var input in row)
            {
                _engine.UpdateVariable(input.Key, input.Value);
            }
            if (firstRow)
            {
                foreach (var kv in doc.InternalVars.Rules)
                {
                    var newField = kv.Name;
                    _engine.SetFormula(kv.Name, kv.Formula, OnUpdate);
                }
                firstRow = false;
            }
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
            yield return result;
        }

    }

}
