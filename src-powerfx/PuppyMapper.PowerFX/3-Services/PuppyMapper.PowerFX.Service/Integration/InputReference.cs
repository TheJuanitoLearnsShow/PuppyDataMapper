namespace PuppyMapper.PowerFX.Service.Integration;

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