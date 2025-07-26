using PuppyMapper.PowerFX.Service;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;

namespace PuppyMapper.Viewmodels;

public partial class MappingSectionViewModel : ReactiveObject
{
    public MappingSectionViewModel()
    {
    }

    public MappingSectionViewModel(MappingSection mappingRules)
    {
        Name = mappingRules.Name;
        Rules = new ObservableCollection<MappingRuleViewModel>(
            mappingRules.Rules.Select(rule => new MappingRuleViewModel(rule)));
    }

    [Reactive] public string Name { get; set; } = string.Empty;
    [Reactive] public ObservableCollection<MappingRuleViewModel> Rules { get; set; } = new();
}
