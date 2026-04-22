namespace SampleWithRoslyn;

public class MapperValidationError
{
    public MapperValidationError(string callerName, string message)
    {
        MethodName = callerName;
        Message = message;
    }

    public string MethodName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}