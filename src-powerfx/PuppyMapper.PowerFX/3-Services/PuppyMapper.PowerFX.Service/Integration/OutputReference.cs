namespace PuppyMapper.PowerFX.Service.Integration;

public class OutputReference
{
    public OutputReference()
    {
        
    }
    public OutputReference(string outputId, IHaveOutputOptions outputOptions)
    {
        OutputId = outputId;
        OutputOptions = outputOptions;
    }

    public string OutputId { get; set; } = string.Empty;
    public IHaveOutputOptions OutputOptions { get; set; }
}