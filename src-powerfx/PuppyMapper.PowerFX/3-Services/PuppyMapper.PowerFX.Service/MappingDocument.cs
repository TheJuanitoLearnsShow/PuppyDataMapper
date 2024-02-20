namespace PuppyMapper.PowerFX.Service;

public class MappingDocument(string documentName, IEnumerable<MappingSection> sections)
{
    readonly MappingSection _internalVars = sections.Where(s => s.Name == "VARIABLES").FirstOrDefault() ?? MappingSection.Blank("VARIABLES");
    readonly MappingSection _mappingRules = sections.Where(s => s.Name == "Mapping").FirstOrDefault() ?? MappingSection.Blank("Mapping");

    public MappingSection MappingRules => _mappingRules;

    public MappingSection InternalVars => _internalVars;
}
