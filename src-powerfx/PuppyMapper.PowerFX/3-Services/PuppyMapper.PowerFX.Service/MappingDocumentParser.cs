using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service;
// Read a .fx.yaml file and return a group of formulas. 
// This can produce a flat (non-nested) record where each field is defined by a Fx formula.
public class MappingDocumentParser
{
    // TODO - share this with Yaml parser in https://github.com/microsoft/PowerApps-Language-Tooling 
    // File is "Name: =formula"
    // Should make this a .fx.yaml
    public static IEnumerable<MappingSection> ParseSections(StreamReader lines)
    {
        var line = lines.ReadLine();
        while(line != null)
        {
            if (IsStartSection(line))
            {
                var (newSection, lineLastRead) = ParseSection(line, lines);
                yield return newSection;
                line = lineLastRead;
            }
            else
            {
                line = lines.ReadLine();
            }
        }
    }

    public static (MappingSection section, string lastLine) ParseSection(string sectionLine, StreamReader lines)
    {
        (MappingRule newRule, string lastLine) ParseRule(string firstLine)
        {
            var lineParts = firstLine.Split(":=");
            var name = (lineParts.FirstOrDefault() ?? "_").Trim();
            var restParts = lineParts.Last().Split("//");
            var formula = restParts.First();
            var comments = restParts.Length > 1 ? restParts.Last() : string.Empty;
            var nextLine = lines.ReadLine();
            while (nextLine != null && !IsStartSection(nextLine) && !IsStartRule(nextLine))
            {
                comments += Environment.NewLine + nextLine.Trim().TrimStart('/');
                nextLine = lines.ReadLine();
            }
            return (new MappingRule(name, formula, comments), nextLine);
        }
        var sectionName = sectionLine[11..];
        var line = lines.ReadLine();
        if (line == null) return (MappingSection.Blank(sectionName.Trim()), line);
        var mappingRules = new List<MappingRule>();
        while (line != null && !IsStartSection(line))
        {
            if (IsStartRule(line))
            {
                var (newRule, lineLastRead) = ParseRule(line);
                mappingRules.Add(newRule);
                line = lineLastRead;
            } 
            else
            {
                line = lines.ReadLine();
            }
        }
        return (new MappingSection(sectionName.Trim(), mappingRules.ToImmutableList()) , line);
    }

    private static bool IsStartSection(string nextLine)
    {
        return nextLine.StartsWith("// SECTION ");
    }
    private static bool IsStartRule(string nextLine)
    {
        return nextLine.Split("//").First().Contains(" := ");
    }
}
