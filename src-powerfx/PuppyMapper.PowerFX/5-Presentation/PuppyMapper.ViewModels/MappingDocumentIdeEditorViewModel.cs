using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.PowerFx.Types;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.CustomLangParser;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingDocumentIdeEditorViewModel : ReactiveObject
{
    [Reactive] public string MappingDocument { get; set; } = string.Empty;
    [Reactive] public string InputData { get; set; } = string.Empty;
    [Reactive] public string OutputData { get; set; } = string.Empty;
    
    
    
    [ReactiveCommand]
    private void ExecuteMapping()
    {
        if (string.IsNullOrWhiteSpace(InputData))
        {
            OutputData = "Input data is empty.";
        }
        using var mappingDocumentReader = new StringReader(MappingDocument);
        var doc = MappingDocumentParser.ParseMappingDocument(mappingDocumentReader);
        var dataRow = FormulaValueJSON.FromJson(InputData);
        var mapper = new MapperInterpreter(doc, ImmutableDictionary<string, IMappingDocument>.Empty);
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