using System.Xml.Serialization;

namespace PuppyMapper.PowerFX.Service.XmlParser;

public class MappingDocumentXml
{
    public static string SerializeToXml(MappingDocumentEditDto dto)
    {
        var serializer = new XmlSerializer(typeof(MappingDocumentEditDto));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, dto);
        return stringWriter.ToString();
    }

    public static MappingDocumentEditDto DeserializeFromXml(string xml)
    {
        var serializer = new XmlSerializer(typeof(MappingDocumentEditDto));
        using var stringReader = new StringReader(xml);
        return (MappingDocumentEditDto)serializer.Deserialize(stringReader)!;
    }
}