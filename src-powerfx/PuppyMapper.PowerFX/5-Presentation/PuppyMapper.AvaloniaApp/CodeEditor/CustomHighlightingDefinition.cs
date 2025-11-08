using System.Collections.Generic;
using AvaloniaEdit.Highlighting;

namespace PuppyMapper.AvaloniaApp.CodeEditor;

public class CustomHighlightingDefinition : IHighlightingDefinition
{
    public string Name { get; }
    public HighlightingRuleSet MainRuleSet { get; }
    public IEnumerable<HighlightingColor> NamedHighlightingColors => new List<HighlightingColor>();
    public IDictionary<string, string> Properties { get; }
    public IDictionary<string, HighlightingColor> NamedHighlightingColorsDictionary => new Dictionary<string, HighlightingColor>();

    public CustomHighlightingDefinition(string name, HighlightingRuleSet rules)
    {
        Name = name;
        MainRuleSet = rules;
    }

    public HighlightingColor GetNamedColor(string name) => null;
    public HighlightingRuleSet GetNamedRuleSet(string name) => null;
}