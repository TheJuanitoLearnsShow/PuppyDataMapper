using Microsoft.PowerFx.Syntax;
using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;

// Describe each rule in the file. 
public class MappingSection
{
    public MappingSection()
    {
    }

    public MappingSection(string name,
        MappingRule[] rules)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Rules = rules ?? throw new ArgumentNullException(nameof(rules));
    }
    public static MappingSection Blank(string sectionName) => new MappingSection(sectionName, []);
    public string Name { get; set; }
    public MappingRule[] Rules { get; set; }

    public void Deconstruct(out string Name, out MappingRule[] Rules)
    {
        Name = this.Name;
        Rules = this.Rules;
    }
}
