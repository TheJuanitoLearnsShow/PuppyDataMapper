using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service.YmlParser;

public class MappingDocumentYml
{
    public string DocumentName { get; set; } = string.Empty;
    public IEnumerable<MappingRule> Variables { get; set;  } = [];
    public IEnumerable<MappingRule> MappingRules  { get; set;  } = [];

    public IEnumerable<MappingInput> MappingInputs { get; set; } = [];
    private MappingOutputType MappingOutputType { get; set; } = new ("object");

    public MappingDocument ToMappingDocument()
    {
        return new MappingDocument(DocumentName, [], MappingInputs, MappingOutputType,
            Variables.ToImmutableList(), MappingRules.ToImmutableList());
    }
}