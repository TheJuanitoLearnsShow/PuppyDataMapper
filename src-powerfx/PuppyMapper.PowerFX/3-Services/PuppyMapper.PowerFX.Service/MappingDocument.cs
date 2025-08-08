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

    public MappingInput[] MappingInputs { get; } = [..mappingInputs]; 
        // TODO we might not need to specify inputs or Outputs as they could be part of the GUI. They would be listed in a seprate file
        // or from the GUI perspective, they could be saved into the JSON file
    public MappingOutputType MappingOutputType { get; } = mappingOutputType;
}
