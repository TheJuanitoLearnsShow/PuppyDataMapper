using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PuppyDataMapper;

[Generator]
public class DataMapperGenerator : ISourceGenerator
{

    public static string GenerateClassFile(string className, string filePath)
    {
        StringBuilder sb = new StringBuilder();
        //XmlDocument doc = new XmlDocument();
        //doc.Load(filePath);

        //var mapInstructions = doc.SelectNodes("/fieldMaps/map");

        //var mappingMethods = mapInstructions.Cast<XmlNode>().Select(GenerateMappingMethod);
        XmlSerializer serializer = new XmlSerializer(typeof(Mapper));
     
        using var reader = new StreamReader(filePath);
        
        var test = (serializer.Deserialize(reader) as Mapper)!;
        
       
        //// Usings
        sb.Append(@"
namespace Mapper;
using System.Collections.Generic;

");
        var classCode = test.ToCode();
        sb.Append(classCode);
        //foreach ( var mappingMethoid in test.Maps.FieldMaps.SelectMany(r => r.Inputs.Input))
        //{
        //    sb.AppendLine($"    {mappingMethoid.Formula}");
        //}
        

        // Close things (class, namespace)
        //sb.Append("              }\n}\n");
        return sb.ToString();

    }

    private static string GenerateMappingMethod(XmlNode mapNode)
    {
        var outputNode = mapNode.SelectSingleNode("output");
        var outputType = outputNode?.Attributes?["type"]?.Value ?? "object";
        var mapName = mapNode?.Attributes?["name"]?.Value ?? "Unkown";
        var inputs = mapNode.SelectNodes("inputs/input").Cast<XmlNode>().Select(GenerateInputParam).ToList();
        var paramListComments = string.Join("\n", inputs.Select(r => r.Comments));
        var paramListCode = string.Join(", ", inputs.Select(r => r.ParamSource));
        var customLogic = mapNode?.SelectSingleNode("customLogic")?.InnerText ?? string.Empty;
        return $"""
            /* {customLogic} */
            {paramListComments}
            public partial {outputType} Map{mapName}({paramListCode});
            """;
    }

    private static (string Comments,string ParamSource) GenerateInputParam(XmlNode inputNode)
    {
        var inputName = inputNode?.Attributes?["name"]?.Value ?? "unkown";
        var description = inputNode?.SelectSingleNode("comments")?.InnerText ?? string.Empty;
        var source = inputNode?.SelectSingleNode("source")?.InnerText ?? string.Empty;
        return (
            $"/// <param name=\"{inputName}\">{description}. Source: {source}</param>",
            $"string {inputName}");
    }

    static string StringToValidPropertyName(string s)
    {
        s = s.Trim();
        s = char.IsLetter(s[0]) ? char.ToUpper(s[0]) + s.Substring(1) : s;
        s = char.IsDigit(s.Trim()[0]) ? "_" + s : s;
        s = new string(s.Select(ch => char.IsDigit(ch) || char.IsLetter(ch) ? ch : '_').ToArray());
        return s;
    }

    static IEnumerable<(string, string)> SourceFilesFromAdditionalFile(AdditionalText file)
    {
        string className = Path.GetFileNameWithoutExtension(file.Path).Replace("mapping", string.Empty, StringComparison.OrdinalIgnoreCase);
        return new (string, string)[] { (className, GenerateClassFile(className, file.Path)) };
    }

    static IEnumerable<(string, string)> SourceFilesFromAdditionalFiles(IEnumerable<AdditionalText> pathsData)
        => pathsData.SelectMany(SourceFilesFromAdditionalFile);

    static IEnumerable<AdditionalText> GetLoadOptions(GeneratorExecutionContext context)
    {
        foreach (AdditionalText file in context.AdditionalFiles)
        {
            bool pathMatchesMappingFile = Path.GetExtension(file.Path).Equals(".xml", StringComparison.OrdinalIgnoreCase)
                            && Path.GetFileName(file.Path).StartsWith("mapping", StringComparison.OrdinalIgnoreCase);
            if (pathMatchesMappingFile)
            {
                yield return file;
            }
        }
    }

    public void Execute(GeneratorExecutionContext context)
    {
        IEnumerable<AdditionalText> filesToProcess = GetLoadOptions(context);
        IEnumerable<(string, string)> nameCodeSequence = SourceFilesFromAdditionalFiles(filesToProcess);
        foreach ((string name, string code) in nameCodeSequence)
            context.AddSource($"Csv_{name}.g.cs", SourceText.From(code, Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}