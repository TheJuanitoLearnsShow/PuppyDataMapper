namespace PuppyMapper.PowerFX.Service.Reducer;

public interface IGroupingDocument
{
    public MappingSection InternalVars { get; set;}
    MappingRule[] GroupByRules { get; set; }
    MappingRule[] SummaryRules { get; set; }
}