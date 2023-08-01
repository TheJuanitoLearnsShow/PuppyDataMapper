using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "input")]
public class MapperInput
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "type")]
    public string Type { get; set; } = string.Empty;

    [XmlElement(ElementName = "referenceName")]
    public string ReferenceName { get; set; } = string.Empty;


    [XmlElement(ElementName = "comments")]
    public string Comments { get; set; } = string.Empty;

}
