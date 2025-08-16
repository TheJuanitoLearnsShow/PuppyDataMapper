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
    private string _varsCode = string.Empty;
    private string _rulesCode = string.Empty;
    [Reactive] public MappingDocumentEditDto MappingDocument { get; set; } = new();
    [Reactive] public string InputData { get; set; } = string.Empty;
    [Reactive] public string OutputData { get; set; } = string.Empty;
    [Reactive] public string MappingFilePath { get; set; } = string.Empty;

    [Reactive]
    public string VarsCode
    {
        get => _varsCode;
        set
        {
            _varsCode = value;
            MappingDocument.InternalVarsCode = _varsCode;
        }
    }

    [Reactive]
    public string RulesCode
    {
        get => _rulesCode;
        set
        {
            _rulesCode = value;
            MappingDocument.MappingRulesCode = _rulesCode;
        }
    }
    
    [ReactiveCommand]
    private async Task LoadMapping(string filePath)
    {
        var xml = await File.ReadAllTextAsync(filePath);
        var deserializedDoc = MappingDocumentXml.DeserializeFromXml(xml);
        MappingDocument = deserializedDoc;
    }

    [ReactiveCommand]
    private void ExecuteMapping()
    {
        if (string.IsNullOrWhiteSpace(InputData))
        {
            OutputData = "Input data is empty.";
        }
        var dataRow = FormulaValueJSON.FromJson(InputData);
        var mapper = new MapperInterpreter(MappingDocument, ImmutableDictionary<string, IMappingDocument>.Empty);
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