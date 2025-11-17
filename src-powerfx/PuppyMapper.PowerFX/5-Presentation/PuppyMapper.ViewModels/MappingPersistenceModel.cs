using System.Xml.Serialization;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.Integration;

namespace PuppyMapper.Viewmodels;

public class MappingPersistenceModel
{
    public MappingDocumentEditDto Document { get; set; } = new();
    public FromMemoryStateOptions[] MemoryInputs { get; set; } = [];
    public FromCSVFileOptions[] CSVInputs { get; set; } = [];

    public IHaveInputOptions[] GetAllInputs()
    {
        var inputs = new List<IHaveInputOptions>();
        inputs.AddRange(MemoryInputs);
        inputs.AddRange(CSVInputs);
        return inputs.ToArray();
    }

    public ToMemoryStateOptions[] MemoryOutputs { get; set; } = [];
    public ToCSVFileOptions[] CsvOutputs { get; set; } = [];

    public string Name { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;

    public IHaveOutputOptions[] GetAllOutputs()
    {
        var outputs = new List<IHaveOutputOptions>();
        outputs.AddRange(MemoryOutputs);
        outputs.AddRange(CsvOutputs);
        return outputs.ToArray();
    }


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