namespace PuppyMapper.PowerFX.Service;

public class MappingInput
{
    public MappingInput()
    {
    }
    public MappingInput(string inputName, string inputType)
    {
        InputName = inputName;
        InputType = inputType;
    }

    public string InputName { get; init; } = string.Empty;
    public string InputType { get; init; } = string.Empty;

    public void Deconstruct(out string InputName, out string InputType)
    {
        InputName = this.InputName;
        InputType = this.InputType;
    }
}