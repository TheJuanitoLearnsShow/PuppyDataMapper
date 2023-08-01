using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "input")]
public class MapperInput
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "type")]
    public string Type { get; set; }

    [XmlElement(ElementName = "referenceName")]
    public string ReferenceName { get; set; }

}
