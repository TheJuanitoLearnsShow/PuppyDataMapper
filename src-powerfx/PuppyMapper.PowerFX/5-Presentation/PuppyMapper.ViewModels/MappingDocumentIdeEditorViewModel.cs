using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.PowerFx.Types;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.CustomLangParser;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.PowerFX.Service.XmlParser;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using LisOfInputs =
    System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<(string Key,
        Microsoft.PowerFx.Types.FormulaValue Value)>>;

namespace PuppyMapper.Viewmodels;

public partial class MappingDocumentIdeEditorViewModel : ReactiveObject
{
    private MappingDocumentEditDto _mappingDocument = new();
    [Reactive] private string _inputData = string.Empty;
    [Reactive] private string _outputData = string.Empty;
    [Reactive] private string _mappingFilePath = string.Empty;

    [Reactive] private string _varsCode = string.Empty;

    [Reactive] private string _rulesCode = string.Empty;

    public ObservableCollection<IHaveInputOptions> Inputs { get; set; } = [];
    
    
    [ReactiveCommand]
    private async Task SaveMapping()
    {
        var filePath = MappingFilePath;
        try
        {
            SyncToMappingDocument();
            
            var persistenceModel = new MappingPersistenceModel()
            {
                Document = _mappingDocument,
                CSVInputs = Inputs.OfType<FromCSVFileOptions>().ToArray()
            };
            var xml = MappingPersistenceModel.SerializeToXml(persistenceModel);
            await File.WriteAllTextAsync(filePath, xml);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    [ReactiveCommand]
    private async Task LoadMapping()
    {
        var filePath = MappingFilePath;
        try
        {
            var xml = await File.ReadAllTextAsync(filePath);
            var deserializedDoc = MappingPersistenceModel.DeserializeFromXml(xml);
            _mappingDocument = deserializedDoc.Document;
            Inputs.Clear();
            foreach (var input in deserializedDoc.CSVInputs)
            {
                Inputs.Add(input);
            }
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
    private async Task ExecuteMapping()
    {
        var inputProvider = GetInputProvider();
        if (inputProvider == null)
        {
            OutputData = "Input data is empty.";
            return;
        }

        InputData = await inputProvider.GetRecordAsJson() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(InputData))
        {
            OutputData = "Input data is empty.";
            return;
        }

        var dataRow = FormulaValueJSON.FromJson(InputData);
        SyncToMappingDocument();
        var mapper = new MapperInterpreter(_mappingDocument, ImmutableDictionary<string, IMappingDocument>.Empty);
        var result = mapper.MapRecords([
            [("input", dataRow)]
        ]).ToList();
        var resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null
        });
        OutputData = resultJson;
    }

    [ReactiveCommand]
    private async Task<List<Dictionary<string, object>>> ExecuteFullMapping()
    {
        SyncToMappingDocument();
        var mapper = new MapperInterpreter(_mappingDocument, ImmutableDictionary<string, IMappingDocument>.Empty);
        var inputProvider = GetInputProvider();
        if (inputProvider == null)
        {
            return [];
        }

        var inputRows = await GetRows();
        var resultForRows = mapper.MapRecords(
            inputRows
        ).ToList();
        var resultJson = JsonSerializer.Serialize(resultForRows, new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null
        });
        await File.WriteAllTextAsync("output.json", resultJson);
        return resultForRows;
    }

    private async Task<IEnumerable<IEnumerable<(string Key, FormulaValue Value)>>> GetRows()
    {
        var rows = new List<IEnumerable<(string Key, FormulaValue Value)>>();
        var inputProvider = GetInputProvider();
        if (inputProvider == null)
        {
            return [];
        }

        var recordRaw = await inputProvider.GetRecordAsJson();
        while (!string.IsNullOrEmpty(recordRaw))
        {
            var dataRow = FormulaValueJSON.FromJson(InputData);
            rows.Add([("input", dataRow)]);
        }

        return rows;
    }

    private IProvideInputData? GetInputProvider()
    {
        var inputsUsed = Inputs.Where(i => 
            _mappingDocument.MappingInputs.Any(mi => mi.InputId == i.InputId)).ToList();
        var firstInput = inputsUsed.FirstOrDefault();
        var inputProvider = firstInput?.BuildProvider();
        return inputProvider;
    }
}