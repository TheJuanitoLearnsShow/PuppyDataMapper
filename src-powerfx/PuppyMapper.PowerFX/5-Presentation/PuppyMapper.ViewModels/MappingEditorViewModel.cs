using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.PowerFx.Types;
using PuppyMapper.PowerFX.Service;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class MappingEditorViewModel: ReactiveObject
{
    [Reactive] public MappingDocumentViewModel MappingDocument { get; set; } = new();
    [Reactive] public string InputData { get; set; } = string.Empty;
    [Reactive] public string OutputData { get; set; } = string.Empty;

    [ReactiveCommand]
    private async Task LoadDocument(string filePath)
    {
        MappingDocument = new MappingDocumentViewModel();
        MappingDocument.LoadDocumentCommand.Execute(filePath).Subscribe();
    }

    [ReactiveCommand]
    private void ExecuteMapping()
    {
        if (string.IsNullOrWhiteSpace(InputData))
        {
            OutputData = "Input data is empty.";
        }
        var dataRow = FormulaValueJSON.FromJson(InputData);
        var mapper = new MapperInterpreter(MappingDocument.GetMappingDocument(), ImmutableDictionary<string, IMappingDocument>.Empty);
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