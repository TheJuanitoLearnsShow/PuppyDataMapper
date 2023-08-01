using System.Xml;
using System.Xml.Serialization;

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
}
