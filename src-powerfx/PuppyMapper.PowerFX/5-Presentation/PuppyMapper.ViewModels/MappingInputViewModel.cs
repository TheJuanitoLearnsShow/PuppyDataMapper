using PuppyMapper.PowerFX.Service;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingInputViewModel : ReactiveObject
{
    public MappingInputViewModel()
    {

    }
    public MappingInputViewModel(MappingInput input)
    {
        InputName = input.InputName;
        InputType = input.InputId;
    }

    [Reactive] public string InputName { get; set; }
    [Reactive] public string InputType { get; set; }

    public MappingInput GetMappingInput()
    {
        return new MappingInput(InputName, InputType);
    }
}
