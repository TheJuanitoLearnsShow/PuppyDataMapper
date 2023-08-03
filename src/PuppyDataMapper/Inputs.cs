using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "inputs")]
public class Inputs
{
    [XmlElement(ElementName = "input")]
    public List<FieldInput> Input { get; set; } = new List<FieldInput>();

    [XmlAttribute(attributeName: "generate-code")]
    public bool GenerateCode { get; set; }
}

public class FieldInput
{
    [XmlElement(ElementName = "source")]
    public string Source { get; set; } = string.Empty;

    [XmlElement(ElementName = "formula")]
    public string Formula { get; set; } = string.Empty;

    [XmlElement(ElementName = "comments")]
    public string Comments { get; set; } = string.Empty;

    [XmlElement(ElementName = "referenceName")]
    public string ReferenceName { get; set; } = string.Empty;
}