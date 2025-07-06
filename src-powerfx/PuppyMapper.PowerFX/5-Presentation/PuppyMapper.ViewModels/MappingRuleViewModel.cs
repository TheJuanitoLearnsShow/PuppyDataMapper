using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingRuleViewModel : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public string Formula { get; set; }
    [Reactive] public string Comments { get; set; }
}
