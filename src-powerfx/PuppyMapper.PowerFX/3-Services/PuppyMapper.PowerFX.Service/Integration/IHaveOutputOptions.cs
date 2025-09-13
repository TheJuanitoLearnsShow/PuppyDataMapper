namespace PuppyMapper.PowerFX.Service.Integration;

public interface IHaveOutputOptions
{
    string OutputId { get; set; }
    IProvideOutputData BuildProvider();
}