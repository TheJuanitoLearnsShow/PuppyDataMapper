using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.PowerFx.Types;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.CustomLangParser;
using PuppyMapper.PowerFX.Service.XmlParser;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingDocumentIdeEditorViewModel : ReactiveObject
{
    private MappingDocumentEditDto _mappingDocument = new();
    [Reactive] private string _inputData = string.Empty;
    [Reactive] private string _outputData = string.Empty;
    [Reactive] private string _mappingFilePath = string.Empty;

    [Reactive]
    private string _varsCode = string.Empty;

    [Reactive]
    private string _rulesCode = string.Empty;

    [ReactiveCommand]
    private async Task LoadMapping()
    {
        var filePath = MappingFilePath;
        try
        {
            var xml = await File.ReadAllTextAsync(filePath);
            var deserializedDoc = MappingDocumentXml.DeserializeFromXml(xml);
            _mappingDocument = deserializedDoc;
            SyncFromMappingDocument();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void SyncFromMappingDocument()
    {
        VarsCode = _mappingDocument.InternalVarsCode;
        RulesCode = _mappingDocument.MappingRulesCode;
    }

    private void SyncToMappingDocument()
    {
        _mappingDocument.InternalVarsCode = VarsCode;
        _mappingDocument.MappingRulesCode = RulesCode;
    }
    [ReactiveCommand]
    private void ExecuteMapping()
    {
        if (string.IsNullOrWhiteSpace(InputData))
        {
            OutputData = "Input data is empty.";
        }
        var dataRow = FormulaValueJSON.FromJson(InputData);
        SyncToMappingDocument();
        var mapper = new MapperInterpreter(_mappingDocument, ImmutableDictionary<string, IMappingDocument>.Empty);
        var result = mapper.MapRecords([
            [("input", dataRow)]
        ]).ToList();
        var resultJson = JsonSerializer.Serialize(result,  new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null
        });
        OutputData = resultJson;
    }
}