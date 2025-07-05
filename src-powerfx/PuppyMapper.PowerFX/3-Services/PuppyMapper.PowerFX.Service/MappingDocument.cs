using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

public class MappingDocument(string documentName, IEnumerable<MappingSection> sections, 
    IEnumerable<MappingInput> mappingInputs, MappingOutputType mappingOutputType,
    IImmutableList<MappingRule>? Variables = null,
    IImmutableList<MappingRule>? MappingRules = null) : IMappingDocument
{
    public const string VariablesSectionName = "VARIABLES";
    private const string MappingSectionName = "Mapping";

    public MappingSection MappingRules { get; } = MappingRules == null ?
        sections.FirstOrDefault(s => s.Name == MappingSectionName) ?? MappingSection.Blank(MappingSectionName)
        : new MappingSection(VariablesSectionName, MappingRules.ToArray());

    public MappingSection InternalVars { get; } = Variables == null
        ? sections.FirstOrDefault(s => s.Name == VariablesSectionName) ?? MappingSection.Blank(VariablesSectionName)
        : new MappingSection(VariablesSectionName, Variables.ToArray());

    public ImmutableArray<MappingInput> MappingInputs { get; } = [..mappingInputs];
    public MappingOutputType MappingOutputType { get; } = mappingOutputType;
}
