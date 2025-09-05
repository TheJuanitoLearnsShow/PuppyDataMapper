using System.Xml.Serialization;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.Viewmodels;

public class MappingPersistenceModel
{
    public MappingDocumentEditDto Document { get; set; } = new();
    public FromCSVFileOptions[] CSVInputs { get; set; } = [];
    
    public static MappingPersistenceModel DeserializeFromXml(string xml)
    {
        var serializer = new XmlSerializer(typeof(MappingPersistenceModel));
        using var stringReader = new StringReader(xml);
        return (MappingPersistenceModel)serializer.Deserialize(stringReader)!;
    }
    
    public static string SerializeToXml(MappingPersistenceModel dto)
    {
        var serializer = new XmlSerializer(typeof(MappingPersistenceModel));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, dto);
        return stringWriter.ToString();
    }
     
}