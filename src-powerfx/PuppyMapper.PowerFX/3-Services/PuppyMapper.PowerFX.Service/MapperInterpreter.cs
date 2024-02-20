using Microsoft.PowerFx;
using Microsoft.PowerFx.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppyMapper.PowerFX.Service;

public class MapperInterpreter
{
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
}
