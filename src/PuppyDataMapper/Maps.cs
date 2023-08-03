using System.Xml.Linq;
using System.Xml.Serialization;

namespace PuppyDataMapper;

[XmlRoot(ElementName = "maps")]
public class Maps
{

    [XmlElement(ElementName = "map")]
    public List<FieldMap> FieldMaps { get; set; } = new List<FieldMap>();

    public string ToCode(MapperInputCollection inputs)
    {
        var fieldMethods = string.Join("\n ", FieldMaps.Select(r => r.ToCode(inputs)));
        return $"\n" +
        $"""
            {fieldMethods}
        """ +
        "\n}";
    }
}
