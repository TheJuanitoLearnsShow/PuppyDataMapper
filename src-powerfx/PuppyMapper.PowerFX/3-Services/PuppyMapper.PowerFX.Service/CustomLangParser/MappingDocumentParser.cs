﻿using System.Collections.Immutable;

namespace PuppyMapper.PowerFX.Service.CustomLangParser;
// Read a .fx.yaml file and return a group of formulas. 
// This can produce a flat (non-nested) record where each field is defined by a Fx formula.
public class MappingDocumentParser
{
    public static MappingDocument ParseMappingDocument(StreamReader fileContents)
    {
        List<MappingSection> sections = [];
        List<MappingInput> inputs = [];
        MappingOutputType outputType = new("Dictionary");
        var line = fileContents.ReadLine();
        while (line != null)
        {
            var lineSpan = line.AsSpan();
            if (IsStartSection(lineSpan))
            {
                var sectionName = lineSpan[11..].Trim();
                switch (sectionName)
                {
                    case "INPUTS":
                        inputs = ParseInputs(fileContents).ToList();
                        break;
                    case "OUTPUT":
                        outputType = ParseOutput(fileContents);
                        sections = ParseSections(fileContents).ToList();
                        break;
                    default:
                        break;
                }
            }
            line = fileContents.ReadLine();
        }
        return new MappingDocument("test", sections, inputs, outputType);
    }

    public static IEnumerable<MappingInput> ParseInputs(StreamReader lines)
    {
        var line = lines.ReadLine();
        while (!string.IsNullOrWhiteSpace(line)) // force blank line between sections
        {
            var input = ParseInputLine(line.Trim());
            if (input != null)
            {
                yield return input;
            }
            line = lines.ReadLine();
        }
    }
    public static MappingOutputType ParseOutput(StreamReader lines)
    {
        var line = lines.ReadLine();
        return new(line.Trim());
    }
    // TODO - share this with Yaml parser in https://github.com/microsoft/PowerApps-Language-Tooling 
    // File is "Name: =formula"
    // Should make this a .fx.yaml
    public static IEnumerable<MappingSection> ParseSections(StreamReader lines)
    {
        var line = lines.ReadLine();
        while (line != null)
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
            return (new MappingRule(name, formula.Trim(), comments), nextLine);
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
        return (new MappingSection(sectionName.Trim(), mappingRules.ToImmutableList()), line);
    }

    private static bool IsStartSection(ReadOnlySpan<char> nextLine)
    {
        return nextLine.StartsWith("// SECTION ");
    }
    private static MappingInput? ParseInputLine(string line)
    {
        var parts = line.Split(" ");
        if (parts.Length >= 2)
        {
            return new(parts[1], parts[0]);
        }
        return null;
    }
    private static bool IsStartRule(string nextLine)
    {
        return nextLine.Split("//").First().Contains(" := ");
    }
}
