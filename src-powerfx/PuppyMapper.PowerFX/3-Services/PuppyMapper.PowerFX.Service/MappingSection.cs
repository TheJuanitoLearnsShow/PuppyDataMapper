using Microsoft.PowerFx.Syntax;
using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

// Describe each rule in the file. 
public record MappingSection(string Name,
     IImmutableList<MappingRule> Rules)
{
    public static MappingSection Blank(string sectionName) => new MappingSection(sectionName, []);
}
