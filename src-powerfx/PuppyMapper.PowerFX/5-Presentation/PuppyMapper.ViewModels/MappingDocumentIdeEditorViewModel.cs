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

public partial class MappingDocumentIdeEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private MappingDocumentEditDto _mappingDocument = new();
    [Reactive] private string _inputData = string.Empty;
    [Reactive] private string _outputData = string.Empty;
    [Reactive] private string _mappingFilePath = string.Empty;

    [Reactive] private string _varsCode = string.Empty;

    [Reactive] private string _rulesCode = string.Empty;
    [Reactive] private ViewModels.Inputs.InputEditorViewModel _inputEditor;

    public ObservableCollection<IHaveInputOptions> Inputs { get; set; } = [];
    [Reactive] private IHaveInputOptions? _selectedInput = null;
    
    public ObservableCollection<IHaveOutputOptions> Outputs { get; set; } = [];
    
    public MappingDocumentIdeEditorViewModel()
    {
    }
    
    public MappingDocumentIdeEditorViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _inputEditor = new ViewModels.Inputs.InputEditorViewModel(this, hostScreen);
    }

    [ReactiveCommand]
    private void AddInput()
    {
        HostScreen.Router.Navigate.Execute(_inputEditor);
    }
    [ReactiveCommand]
    private void ModifyInput()
    {
        if (_selectedInput != null)
        {
            _inputEditor.ModifyInput(_selectedInput);
        }
    }
    
    
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
                CSVInputs = Inputs.OfType<FromCSVFileOptions>().ToArray(),
                MemoryInputs = Inputs.OfType<FromMemoryStateOptions>().ToArray(),
                MemoryOutputs = Outputs.OfType<ToMemoryStateOptions>().ToArray(),
                CsvOutputs = Outputs.OfType<ToCSVFileOptions>().ToArray(),
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
            foreach (var input in deserializedDoc.GetAllInputs())
            {
                Inputs.Add(input);
            }
            Outputs.Clear();
            foreach (var output in deserializedDoc.GetAllOutputs())
            {
                Outputs.Add(output);
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

        foreach (var outputOptions in Outputs)
        {
            var outputProvider = outputOptions.BuildProvider();
            await outputProvider.OutputData(result, simulateOnly: true);
            OutputData = outputProvider.GetDisplayData();
        }
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
        
        foreach (var outputOptions in Outputs)
        {
            var outputProvider = outputOptions.BuildProvider();
            await outputProvider.OutputData(resultForRows);
            OutputData = outputProvider.GetDisplayData();
        }
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
            var dataRow = FormulaValueJSON.FromJson(recordRaw);
            rows.Add([("input", dataRow)]);
            recordRaw = await inputProvider.GetRecordAsJson();
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
    
    public string? UrlPathSegment { get; } = "mappingDocumentEditor";
    public IScreen HostScreen { get; }

    public void UpdateInput(IHaveInputOptions newOptions)
    {
        var foundItem = Inputs.FirstOrDefault(i => i.InputId == newOptions.InputId);
        if (foundItem != null)
        {
            Inputs.Remove(foundItem);
        }
        Inputs.Add(newOptions);
    }

    public void UpdateOutput(IHaveOutputOptions newOptions)
    {
        var foundItem = Outputs.FirstOrDefault(i => i.OutputId == newOptions.OutputId);
        if (foundItem != null)
        {
            Outputs.Remove(foundItem);
        }
        Outputs.Add(newOptions);
    }
}