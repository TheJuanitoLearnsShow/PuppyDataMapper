using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class FromCSVFileOptions : IHaveInputOptions
{
    public string FilePath { get; set; } = string.Empty;
    public string InputId { get; set; } = string.Empty;

    public string InputTypeName { get; } = "CSV";

    public IProvideInputData BuildProvider()
    {
        return new FromCSVFile(this);
    }
}