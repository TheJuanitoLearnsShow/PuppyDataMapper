using YamlDotNet.Serialization;

namespace PuppyMapper.PowerFX.Service.YmlParser;

public class YmlParser
{
    public MappingDocument ParseMappingDocument(StreamReader fileContents)
    {
        var deserializer = new Deserializer();
        var ymlData = deserializer.Deserialize<MappingDocumentYml>(fileContents);
        return ymlData.ToMappingDocument();
    }
}