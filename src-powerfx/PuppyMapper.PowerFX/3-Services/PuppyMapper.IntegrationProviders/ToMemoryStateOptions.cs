using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.IntegrationProviders;

public class ToMemoryStateOptions : IHaveOutputOptions
{
    public string PropertyPath { get; set; } = string.Empty;
    public string OutputId { get; set; } = string.Empty;

    public IProvideOutputData BuildProvider()
    {
        return new ToMemoryState(this);
    }
}