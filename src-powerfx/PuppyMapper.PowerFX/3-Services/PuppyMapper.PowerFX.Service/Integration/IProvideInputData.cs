namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideInputData
{
    Task<string?> GetRecordAsJson();
}

public interface IHaveInputOptions
{
    IProvideInputData BuildProvider();
}

public class InputReference
{
    public string InputId { get; set; } = string.Empty;
    public IHaveInputOptions InputOptions { get; set; }
}