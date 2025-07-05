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
    public MappingSection(string name,
        IImmutableList<MappingRule> rules)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Rules = rules.ToArray() ?? throw new ArgumentNullException(nameof(rules));
    }
    public static MappingSection Blank(string sectionName) => 
        new MappingSection(sectionName, (MappingRule[])[]);
    public string Name { get; set; }
    public MappingRule[] Rules { get; set; }

    public void Deconstruct(out string name, out MappingRule[] rules)
    {
        name = this.Name;
        rules = this.Rules;
    }
}
