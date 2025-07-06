using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingInputViewModel : ReactiveObject
{
    [Reactive] public string InputName { get; set; }
    [Reactive] public string InputType { get; set; }
}
