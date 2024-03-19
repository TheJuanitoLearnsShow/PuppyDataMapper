
using Microsoft.PowerFx;
using Microsoft.PowerFx.Core.Types;
using Microsoft.PowerFx.Core.Public.Types;
using Microsoft.PowerFx.Types;

namespace PuppyMapper.PowerFX.Service;
public class PowerFxIntegration
{
    public static void RegisterCustomFunction(RecalcEngine engine)
    {

        // Create a custom function that calls the C# method
        var customFunction = new DoMapFunction();

        // Register the custom function with the Power Fx engine
        engine.Config.AddFunction(customFunction);
    }
}
public class DoMapFunction : ReflectionFunction
{
    public RecordValue Execute(StringValue mappingName)
    {
        return FormulaValue.NewRecordFromFields([
            new ("field1", FormulaValue.New("Hi")),
            new ("field2", FormulaValue.New(4)),

            ]);
    }
}