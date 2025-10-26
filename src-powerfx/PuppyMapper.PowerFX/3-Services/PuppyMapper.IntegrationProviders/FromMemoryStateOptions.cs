using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class FromMemoryStateOptions : IHaveInputOptions
{
    public string PropertyPath { get; set; } = string.Empty;
    public string InputId { get; set; } = string.Empty;

    public string InputTypeName { get; } = "Memory State";

    public IProvideInputData BuildProvider()
    {
        return new FromMemoryState(this);
    }
}