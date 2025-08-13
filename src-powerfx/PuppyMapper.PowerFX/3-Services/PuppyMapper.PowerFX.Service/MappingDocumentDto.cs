using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;
    
public class MappingDocumentDto : IMappingDocument
{
    
    public List<MappingRule> MappingRules { get; set; }= new();
    public MappingSection InternalVars { get; set;}= new();
    public MappingInput[] MappingInputs { get; set; } = [];
    public MappingOutputType MappingOutputType { get; set; } = new();
}