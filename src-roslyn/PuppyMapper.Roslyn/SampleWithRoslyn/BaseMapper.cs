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
    protected bool AddValidationErrorIf(bool validationErrorOccurred, string message, [CallerMemberName] string callerName = "")
    {
        if (validationErrorOccurred) {
            Console.WriteLine(message);
            ValidationErrors.Add(new MapperValidationError(callerName, message));
        }
        return validationErrorOccurred;
    }
    protected bool AnyValidationErrorsInCurrentMethod([CallerMemberName] string callerName = "")
    {
        return ValidationErrors.Any(x => x.MethodName == callerName);
    }
    protected void DoMap(Action mappingAssignmentAction, string comment = "", bool? condition = null, List<(bool warningCondition, string)>? warningsIf = null, 
        List<(bool errorCondition, string)>? validationErrorsIf = null)
    {
        if (condition is false)
        {
            return;
        }

        if (warningsIf != null)
        {
            foreach (var warningIf in warningsIf)
            {
                AddWarningIf(warningIf.warningCondition, warningIf.Item2);
            }
        }
        if (validationErrorsIf != null)
        {
            foreach (var validationErrorIf in validationErrorsIf)
            {
                AddValidationErrorIf(validationErrorIf.errorCondition, validationErrorIf.Item2);
            }
        }
        var anyValidationErrors = validationErrorsIf?.Where(x => x.errorCondition).Any() ?? false;
        if (anyValidationErrors)
        {
            return;
        }
        mappingAssignmentAction();
    }
}