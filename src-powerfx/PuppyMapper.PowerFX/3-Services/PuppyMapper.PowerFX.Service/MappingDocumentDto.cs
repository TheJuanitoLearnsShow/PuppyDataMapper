using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

public class MappingSectionDto()
{
    private string _name;
    MappingRule[] _rules
    }
    
public class MappingDocumentDto
{
    
    public MappingSection MappingRules { get; set; }= new();
    public MappingSection InternalVars { get; set;}= new();
    public ImmutableArray<MappingInput> MappingInputs { get; set; }
    public MappingOutputType MappingOutputType { get; set; } = new();
}