namespace PuppyMapper.Roslyn.MethodStrategy;

public sealed class MethodMapInfo
{
    public string MethodName { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<(string Name, string Comment)> ParameterComments { get; set; } = [];
    public List<AssignmentInfo> Assignments { get; set; } = [];
    public string FinalCondition { get; set; } = string.Empty;
    public List<(string Condition, string Message)> Warnings { get; set; } = [];
    public List<(string Condition, string Message)> ValidationErrors { get; set; } = [];
}