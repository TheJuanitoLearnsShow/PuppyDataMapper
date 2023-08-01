using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "mapper")]
public class Mapper
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "inputs")]
    public List<MapperInput> Input { get; set; }

    [XmlElement(ElementName = "maps")]
    public Maps Maps { get; set; }
}
