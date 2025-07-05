namespace PuppyMapper.PowerFX.Service;

public class MappingOutputType
{
    public MappingOutputType(string outputType)
    {
        OutputType = outputType;
    }
    public string OutputType { get; init; }

    public void Deconstruct(out string OutputType)
    {
        OutputType = this.OutputType;
    }
}