using System.Xml;
using System.Xml.Serialization;
using CaseExtensions;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "map")]
public class FieldMap
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "inputs")]
    public Inputs Inputs { get; set; } = new ();

    [XmlElement(ElementName = "outputType")]
    public string OutputType { get; set; } = string.Empty;

    [XmlElement(ElementName = "customLogic")]
    public string CustomLogic { get; set; } = string.Empty;

    [XmlAttribute(attributeName: "generate-code")]
    public bool GenerateCode { get; set; }

    [XmlElement(ElementName = "formula")]
    public string Formula { get; set; } = string.Empty;

    [XmlElement(ElementName = "outputTo")]
    public string OutputTo { get; set; } = string.Empty;

    public string ToCode(MapperInputCollection mapperInputs)
    {
        return $"\n" +
        $"""
            {GenerateMappingMethod(mapperInputs)}
        """;
    }


    private string GenerateMappingMethod(MapperInputCollection mapperInputs)
    {
        var inputs = Inputs.Input.GroupBy(i => i.Source).Select(grp => GenerateInputParam(grp.Key, grp, mapperInputs.GetTypeFor(grp.Key))).ToList();
        var paramListComments = string.Join("\n", inputs.Select(r => r.Comments));
        var paramListCode = string.Join(", ", inputs.Select(r => r.ParamSource));
        if (GenerateCode)
        {

            var varsCode = string.Join(",\n", Inputs.Input
                .Select(i => $"var {i.ReferenceName.ToCamelCase()} = {i.Source.ToCamelCase()}.{i.Formula};"

                ).ToList()
                ); ;
            return $"""
            /* {CustomLogic} */
            {paramListComments}
            public partial {OutputType} {GetMappingMethodName()}({paramListCode})
            """
            + "{\n"
            + varsCode
            + $"\nreturn {Formula};"
            + "\n}\n";
        }

        if (Inputs.GenerateCode)
        {
            var formulaCode = string.Join(",\n", Inputs.Input
                .Select(i => i.Source.ToCamelCase() + "." + i.Formula

                ).ToList()
                );
            return $"""
            /* {CustomLogic} */
            {paramListComments}
            public partial {OutputType} {GetMappingMethodName()}({paramListCode}) =>
                return MapFieldsTo{Name.ToPascalCase()}(
                {formulaCode}
                );
            """;
        }
        else
        {
            return $"""
            /* {CustomLogic} */
            {paramListComments}
            public partial {OutputType} {GetMappingMethodName()}({paramListCode});
            """;
        }
    }

    private (string Comments, string ParamSource) GenerateInputParam(string key, IEnumerable<FieldInput> fieldInputs, string inputType)
    {
        var combinedComments =
            string.Join("\n///   ", fieldInputs.Select(i => "<li>" + i.Formula + ": " + i.Comments + "</li>"));
        return ($"/// <param name=\"{key.ToCamelCase()}\">{combinedComments}.</param>", inputType + " " + key.ToCamelCase());
    }

    internal object GetMappingMethodName()
    {
        return $"Map{Name.ToPascalCase()}";
    }

    internal object GetMappingMethodCall()
    {
        var inputs = Inputs.Input.GroupBy(i => i.Source).Select(grp => grp.Key.ToCamelCase()).ToList();
        var paramListCode = string.Join(", ", inputs);
        return $"Map{Name.ToPascalCase()}({paramListCode})";
    }

}
