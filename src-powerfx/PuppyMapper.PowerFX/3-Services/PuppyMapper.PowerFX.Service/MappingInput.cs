namespace PuppyMapper.PowerFX.Service;

public class MappingInput
{
    public MappingInput()
    {
    }
    public MappingInput(string inputName, string inputId)
    {
        InputName = inputName;
        InputId = inputId;
    }

    public string InputName { get; init; } = string.Empty;
    public string InputId { get; init; } = string.Empty;

    public void Deconstruct(out string InputName, out string InputId)
    {
        InputName = this.InputName;
        InputId = this.InputId;
    }
}