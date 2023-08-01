using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "maps")]
public class Maps
{

    [XmlElement(ElementName = "map")]
    public List<FieldMap> FieldMaps { get; set; }
}
