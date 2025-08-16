using System.Text;

namespace PuppyMapper.PowerFX.Service;

public record MappingRuleDraft(
    string Name,
    StringBuilder Formula,
    StringBuilder Comments)
{
    public MappingRule MapToMappingRule()
    {
        return new MappingRule(Name.Trim(), Formula.ToString().Trim(), Comments.ToString().Trim());
    }
}