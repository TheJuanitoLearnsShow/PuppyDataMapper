using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

public class MappingDocument(string documentName, IEnumerable<MappingSection> sections, 
    IEnumerable<MappingInput> mappingInputs, MappingOutputType mappingOutputType,
    IImmutableList<MappingRule>? Variables = null,
    IImmutableList<MappingRule>? MappingRules = null)
{
    public const string VariablesSectionName = "VARIABLES";
    private const string MappingSectionName = "Mapping";

    private readonly MappingSection _internalVars = Variables == null
        ? sections.FirstOrDefault(s => s.Name == VariablesSectionName) ?? MappingSection.Blank(VariablesSectionName)
        : new MappingSection(VariablesSectionName, Variables);
    readonly MappingSection _mappingRules = MappingRules == null ?
        sections.FirstOrDefault(s => s.Name == MappingSectionName) ?? MappingSection.Blank(MappingSectionName)
        : new MappingSection(VariablesSectionName, MappingRules);

    public MappingSection MappingRules => _mappingRules;

    public MappingSection InternalVars => _internalVars;

    public ImmutableArray<MappingInput> MappingInputs { get; } = [..mappingInputs];
    public MappingOutputType MappingOutputType { get; } = mappingOutputType;
}
