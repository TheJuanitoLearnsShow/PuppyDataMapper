namespace PuppyMapper.PowerFX.Service.Integration;

public interface IHaveInputOptions
{
    string InputId { get; set; }
    string InputTypeName { get; }
    IProvideInputData BuildProvider();
}