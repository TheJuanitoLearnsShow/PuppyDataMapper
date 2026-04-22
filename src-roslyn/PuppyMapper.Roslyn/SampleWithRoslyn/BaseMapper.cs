using System.Runtime.CompilerServices;

namespace SampleWithRoslyn;

public class BaseMapper
{
    protected List<MapperValidationError> ValidationErrors { get; private set; } = [];
    protected List<MapperWarning> Warnings { get; private set; } = [];
    protected void AddWarningIf(bool warningOccurred, string message, [CallerMemberName] string callerName = "")
    {
        if (warningOccurred) {
            Console.WriteLine(message);
            Warnings.Add(new MapperWarning(callerName, message));
        }
    }
    protected void AddValidationErrorIf(bool warningOccurred, string message, [CallerMemberName] string callerName = "")
    {
        if (warningOccurred) {
            Console.WriteLine(message);
            ValidationErrors.Add(new MapperValidationError(callerName, message));
        }
    }
    protected bool AnyValidationErrorsInCurrentMethod([CallerMemberName] string callerName = "")
    {
        return ValidationErrors.Any(x => x.MethodName == callerName);
    }
}