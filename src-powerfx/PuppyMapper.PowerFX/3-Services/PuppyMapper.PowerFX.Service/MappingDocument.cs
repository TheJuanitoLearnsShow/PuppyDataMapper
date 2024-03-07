using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

public class MappingDocument(string documentName, IEnumerable<MappingSection> sections, 
    IEnumerable<MappingInput> mappingInputs, MappingOutputType mappingOutputType)
{
    readonly MappingSection _internalVars = sections.Where(s => s.Name == "VARIABLES").FirstOrDefault() ?? MappingSection.Blank("VARIABLES");
    readonly MappingSection _mappingRules = sections.Where(s => s.Name == "Mapping").FirstOrDefault() ?? MappingSection.Blank("Mapping");

    public MappingSection MappingRules => _mappingRules;

    public MappingSection InternalVars => _internalVars;

    public ImmutableArray<MappingInput> MappingInputs { get; } = mappingInputs.ToImmutableArray();
    public MappingOutputType MappingOutputType { get; } = mappingOutputType;
}
