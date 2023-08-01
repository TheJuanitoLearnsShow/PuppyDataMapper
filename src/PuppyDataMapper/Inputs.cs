using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "inputs")]
public class Inputs
{
    [XmlElement(ElementName = "input")]
    public List<FieldInput> Input { get; set; }
}

public class FieldInput
{
    [XmlElement(ElementName = "source")]
    public string Source { get; set; }

    [XmlElement(ElementName = "formula")]
    public string Formula { get; set; }

    [XmlElement(ElementName = "comments")]
    public string Comments { get; set; }
}