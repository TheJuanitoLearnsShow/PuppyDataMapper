using PuppyMapper.PowerFX.Service;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingRuleViewModel : ReactiveObject
{
    public MappingRuleViewModel()
    {

    }
    public MappingRuleViewModel(MappingRule rule)
    {
        Name = rule.Name;
        Formula = rule.Formula;
        Comments = rule.Comments;
    }

    [Reactive] public string Name { get; set; }
    [Reactive] public string Formula { get; set; }
    [Reactive] public string Comments { get; set; }

    public MappingRule GetMappingRule()
    {
        return new MappingRule(Name, Formula, Comments);
    }
}
