namespace PuppyMapper.PowerFX.Service.Integration;

public interface IHaveInputOptions
{
    string InputId { get; set; }
    IProvideInputData BuildProvider();
}