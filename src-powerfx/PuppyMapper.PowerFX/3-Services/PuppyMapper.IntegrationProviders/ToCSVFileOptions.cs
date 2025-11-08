using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class ToCSVFileOptions : IHaveOutputOptions
{
    public string FilePath { get; set; } = string.Empty;
    public string OutputId { get; set; } = string.Empty;

    public string OutputTypeName { get; } = "CSV";

    public IProvideOutputData BuildProvider()
    {
        return new ToCSVFile(this);
    }
}