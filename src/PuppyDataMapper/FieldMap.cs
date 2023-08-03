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
            public partial {OutputType} Map{Name.ToPascalCase()}({paramListCode})
            """
            + "{"
            + varsCode
            + $"""
                return {Formula};
                );
            """;
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
            public partial {OutputType} Map{Name.ToPascalCase()}({paramListCode}) =>
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
            public partial {OutputType} Map{Name.ToPascalCase()}({paramListCode});
            """;
        }
    }

    private (string Comments, string ParamSource) GenerateInputParam(string key, IEnumerable<FieldInput> fieldInputs, string inputType)
    {
        var combinedComments =
            string.Join("\n///   ", fieldInputs.Select(i => "<li>" + i.Formula + ": " + i.Comments + "</li>"));
        return ($"/// <param name=\"{key.ToCamelCase()}\">{combinedComments}.</param>", inputType + " " + key.ToCamelCase());
    }

    //private (string Comments, string ParamSource) GenerateInputParam(FieldInput input)
    //{
    //    var description = input.Comments;
    //    return (
    //        $"/// <param name=\"{input.Source.ToCamelCase()}\">{description}. Source: {input.Name}</param>",
    //        $"{input.ou} {input.ReferenceName.ToCamelCase()}");
    //}
}
