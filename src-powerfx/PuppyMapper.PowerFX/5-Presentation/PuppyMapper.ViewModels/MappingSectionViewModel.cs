using PuppyMapper.PowerFX.Service;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Data;

namespace PuppyMapper.Viewmodels;

public partial class MappingSectionViewModel : ReactiveObject
{
    public MappingSectionViewModel()
    {
    }

    public MappingSectionViewModel(MappingSection mappingRules)
    {
        Name = mappingRules.Name;
        Rules = new ObservableCollection<MappingRuleViewModel>();

        foreach (var rule in mappingRules.Rules)
        {
            Rules.Add(new MappingRuleViewModel(rule));
        }
    }

    [Reactive] public string Name { get; set; } = string.Empty;
    [Reactive] public ObservableCollection<MappingRuleViewModel> Rules { get; set; } = new();
}
