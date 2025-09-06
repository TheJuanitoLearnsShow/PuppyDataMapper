using System.Collections.Immutable;
using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;

namespace PuppyMapper.PowerFX.Service.Reducer;

public class GrouperInterpreter
{
    private readonly IGroupingDocument _doc;
    private readonly ImmutableDictionary<string, IGroupingDocument> _childMappers;
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
    public static RecordValue MapRecordAsFormulaValue(
        IGroupingDocument doc, FormulaValue input, ImmutableDictionary<string, IGroupingDocument> childMappers)
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

    public GrouperInterpreter(IGroupingDocument doc, ImmutableDictionary<string, IGroupingDocument> childMappers)
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
        IEnumerable<IEnumerable<(string Key, FormulaValue Value)>> rows)
    {
        return MapRecords(_doc, rows);
    }

    /// <summary>
    /// Each row in rows is a collection of a set of fields from multiple inputs. E.g.: the first row might have
    /// two elements: one object/FormulaValue for input1 and another object/FormulaValue for input2.
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="rows"></param>
    /// <returns></returns>
    private IEnumerable<Dictionary<string, object>> MapRecords(IGroupingDocument doc,
        IEnumerable<IEnumerable<(string Key, FormulaValue Value)>> rows)
    {
        var firstRow = true;
        Dictionary<string, object> foldState = new();
        foreach(var row in rows)
        {
            Dictionary<string, object> result = new();
            var groupKeys = new string[doc.GroupByRules.Length];
            foreach (var input in row)
            {
                _engine.UpdateVariable(input.Key, input.Value);
            }
            if (firstRow)
            {
                foreach (var kv in doc.InternalVars.Rules)
                {
                    _engine.SetFormula(kv.Name, kv.Formula, OnUpdate);
                }
                firstRow = false;
            }

            for (var grpKeyIdx = 0; grpKeyIdx < doc.GroupByRules.Length; grpKeyIdx++)
            {
                var groupingRule = doc.GroupByRules[grpKeyIdx];

                var value = _engine.Eval(groupingRule.Formula);
                var keyValue = value?.ToString() ?? string.Empty
                groupKeys[grpKeyIdx] = keyValue;
            }

            yield return result;
        }

    }

}