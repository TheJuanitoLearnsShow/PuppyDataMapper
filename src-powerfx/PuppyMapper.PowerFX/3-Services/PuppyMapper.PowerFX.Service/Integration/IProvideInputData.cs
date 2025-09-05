namespace PuppyMapper.PowerFX.Service.Integration;

public interface IProvideInputData
{
    Task<string?> GetRecordAsJson();
}

public interface IHaveInputOptions
{
    string InputId { get; set; }
    IProvideInputData BuildProvider();
}

public class InputReference
{
    public InputReference()
    {
        
    }
    public InputReference(string inputId, IHaveInputOptions inputOptions)
    {
        InputId = inputId;
        InputOptions = inputOptions;
    }

    public string InputId { get; set; } = string.Empty;
    public IHaveInputOptions InputOptions { get; set; }
}