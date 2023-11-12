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

    [XmlElement(ElementName = "comments")]
    public string Comments { get; set; } = string.Empty;

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
        var inputs = (Inputs?.Input.Any() ?? false) ?
            Inputs.Input.GroupBy(i => i.Source).Select(grp => GenerateInputParam(grp.Key, grp, mapperInputs.GetTypeFor(grp.Key))).ToList()
            : mapperInputs.Inputs.Select(grp => GenerateInputParam(grp.ReferenceName, Array.Empty<FieldInput>(),
                mapperInputs.GetTypeFor(grp.ReferenceName))).ToList()
            ;
        var paramListComments = string.Join("\n", inputs.Select(r => r.Comments));

        var paramListCode = string.Join(", ", inputs.Select(r => r.ParamSource));
        if (GenerateCode)
        {

            var varsCode = string.Join("\n", Inputs.Input
                .Select(i => $"var {i.ReferenceName.ToCamelCase()} = {i.Source.ToCamelCase()}.{i.Formula};"

                ).ToList()
                ); ;
            return $"""
            /// {Comments.ToXmlComments()}
            {paramListComments}
            public {OutputType} {GetMappingMethodName()}({paramListCode})
            """
            + "{\n"
            + varsCode
            + $"\nreturn {Formula};"
            + "\n}\n";
        }

        if (Inputs?.GenerateCode ?? false)
        {
            var formulaCode = string.Join(",\n", Inputs.Input
                .Select(i => i.Source.ToCamelCase() + "." + i.Formula

                ).ToList()
                );
            return $"""
            /// {Comments.ToXmlComments()}
            {paramListComments}
            public {OutputType} {GetMappingMethodName()}({paramListCode}) =>
                MapFieldsTo{Name.ToPascalCase()}(
                {formulaCode}
                );
            """;
        }
        else
        {
            return $"""
            /// {Comments.ToXmlComments()}
            {paramListComments}
            public abstract {OutputType} {GetMappingMethodName()}({paramListCode});
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
        return string.IsNullOrEmpty(Name) ? $"Map{OutputTo.ToPascalCase()}" : $"Map{Name.ToPascalCase()}";
    }

    internal object GetMappingMethodCall(MapperInputCollection mapperInputs)
    {

        var inputs = (Inputs?.Input.Any() ?? false) ?
            Inputs.Input.GroupBy(i => i.Source).Select(grp => grp.Key.ToCamelCase()).ToList()
            : mapperInputs.Inputs.Select(grp => grp.ReferenceName.ToCamelCase()).ToList()
            ;
        var paramListCode = string.Join(", ", inputs);
        return $"{GetMappingMethodName()}({paramListCode})";
    }

}
