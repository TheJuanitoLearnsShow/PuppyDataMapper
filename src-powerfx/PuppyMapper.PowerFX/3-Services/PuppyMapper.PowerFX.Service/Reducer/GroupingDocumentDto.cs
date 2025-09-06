namespace PuppyMapper.PowerFX.Service.Reducer;

/// <summary>
/// Describes how to group an input set of records and the summary/totals to calculate for the leaf group.
/// </summary>
public class GroupingDocumentDto : IGroupingDocument
{
    public MappingSection InternalVars { get; set;}= new();
    public MappingRule[] GroupByRules { get; set; } = [];
    public MappingRule[] SummaryRules { get; set; } = [];
}