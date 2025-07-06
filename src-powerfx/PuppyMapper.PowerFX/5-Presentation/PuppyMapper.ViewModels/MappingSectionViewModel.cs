using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;

namespace PuppyMapper.Viewmodels;

public partial class MappingSectionViewModel : ReactiveObject
{
    [Reactive] public string Name { get; set; } = string.Empty;
    [Reactive] public ObservableCollection<MappingRuleViewModel> Rules { get; set; } = new();
}
