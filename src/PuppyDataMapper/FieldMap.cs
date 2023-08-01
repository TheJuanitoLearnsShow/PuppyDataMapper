using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "map")]
public class FieldMap
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "inputs")]
    public Inputs Inputs { get; set; }

    [XmlElement(ElementName = "outputType")]
    public string OutputType { get; set; }

    [XmlElement(ElementName = "customLogic")]
    public string CustomLogic { get; set; }
}
