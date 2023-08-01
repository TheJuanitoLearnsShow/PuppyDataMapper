﻿using System.Xml;
using System.Xml.Serialization;
using CaseExtensions;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "mapper", Namespace = "http://example/mapper")]
public class Mapper
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "description")]
    public string Description { get; set; } = string.Empty;

    [XmlElement(ElementName = "inputs")]
    public MapperInputCollection Inputs { get; set; } = new ();

    [XmlElement(ElementName = "maps")]
    public Maps Maps { get; set; } = new Maps();


    [XmlElement(ElementName = "outputType")]
    public string OutputType { get; set; } = string.Empty;

    public string ToCode() {
        var mapperInputParams = Inputs.Inputs.Select(GenerateInputParam).ToList();
        var paramListComments = string.Join("\n", mapperInputParams.Select(r => r.Comments));
        var paramListCode = string.Join(", ", mapperInputParams.Select(r => r.ParamSource));

        return $"\npublic partial static class {Name.ToPascalCase()}Mapper \n{{\n " +
        $"""
            {paramListComments}
            public partial static {OutputType.ToPascalCase()} Map({paramListCode});
        """ + 
        "\n}";
    }

    private (string Comments, string ParamSource) GenerateInputParam(MapperInput input)
    {
        var description = input.Comments;
        return (
            $"/// <param name=\"{input.ReferenceName.ToCamelCase()}\">{description}. Source: {input.Name}</param>",
            $"{input.Type} {input.ReferenceName.ToCamelCase()}");
    }
}


[XmlRoot(ElementName = "inputs")]
public class MapperInputCollection
{

    [XmlElement(ElementName = "input")]
    public List<MapperInput> Inputs { get; set; } = new List<MapperInput>();
}