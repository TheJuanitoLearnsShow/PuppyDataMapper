
using Microsoft.PowerFx;
using Microsoft.PowerFx.Core.Types;
using Microsoft.PowerFx.Core.Public.Types;
using Microsoft.PowerFx.Types;

namespace PuppyMapper.PowerFX.Service; 
public class PowerFxIntegration
		{
		    public static void RegisterCustomFunction(RecalcEngine engine)
		    {
		        // Define the function signature
		        var functionSignature = new FormulaType(
		            ReturnType: FormulaType.Number,
		            Parameters: new NamedFormulaType("number1", FormulaType.Number),
		            new NamedFormulaType("number2", FormulaType.Number));
		
		        // Create a custom function that calls the C# method
		        var customFunction = new CustomFunction(
		            "SumOfSquares",
		            functionSignature,
		            (context, args) =>
		            {
		                var num1 = (NumberValue)args[0];
		                var num2 = (NumberValue)args[1];
		                var result = CustomMathFunctions.SumOfSquares(num1.Value, num2.Value);
		                return new NumberValue(result);
		            });
		
		        // Register the custom function with the Power Fx engine
		        engine.add(customFunction);
		    }
		}
