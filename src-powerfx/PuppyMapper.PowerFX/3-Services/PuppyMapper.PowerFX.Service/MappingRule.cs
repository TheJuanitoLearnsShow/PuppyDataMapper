using System.Text;

namespace PuppyMapper.PowerFX.Service;

// Describe each rule in the file. 
public record MappingRule(
    string Name,
    string Formula,
    string Comments);

public record MappingRuleDraft(
    string Name,
    StringBuilder Formula,
    StringBuilder Comments)
{
    public MappingRule MapToMappingRule()
    {
        return new MappingRule(Name, Formula.ToString(), Comments.ToString());
    }
}