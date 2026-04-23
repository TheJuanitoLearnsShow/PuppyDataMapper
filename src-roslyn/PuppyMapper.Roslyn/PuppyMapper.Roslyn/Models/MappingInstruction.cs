namespace PuppyMapper.Roslyn.InlineStrategy;

public class MapInstruction
{
    public string OutputField { get; set; } = string.Empty;
    public string MappedValue { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;

    public List<(string Condition, string Message)> Warnings { get; set; } = new();
    public List<(string Condition, string Message)> ValidationErrors { get; set; } = new();
}