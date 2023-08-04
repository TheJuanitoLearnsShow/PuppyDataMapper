using System.Xml;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "inputs")]
public class MapperInputCollection
{

    [XmlElement(ElementName = "input")]
    public List<MapperInput> Inputs { get; set; } = new List<MapperInput>();

    internal string GetTypeFor(string key)
    {
        return Inputs.FirstOrDefault(i => i.ReferenceName == key)?.Type ?? "object";
    }
}