using System.Collections.ObjectModel;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.ViewModels.Outputs;

public partial class OutputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private const string Memory = "Memory";
    private const string CSV = "CSV";
    public ObservableCollection<string> OutputTypes { get; set; } = [CSV, Memory];
    [Reactive] private string _selectedOutputType = string.Empty;
    [Reactive] private CsvOutputEditorViewModel _csvOutputEditor;
    [Reactive] private MemoryOutputEditorViewModel _memoryOutputEditor;
    [Reactive] private MappingDocumentIdeEditorViewModel _docEditor;

    public OutputEditorViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _docEditor = new(hostScreen);
        _csvOutputEditor = new(this, hostScreen);
        _memoryOutputEditor = new(this, hostScreen);
    }
    public OutputEditorViewModel(MappingDocumentIdeEditorViewModel docEditor, IScreen hostScreen)
    {
        _docEditor = docEditor;
        _csvOutputEditor = new(this, hostScreen);
        _memoryOutputEditor = new(this, hostScreen);
        HostScreen = hostScreen;
    }
    
    [ReactiveCommand]
    private void AddOutput()
    {
        switch (_selectedOutputType)
        {
            case Memory:
                HostScreen.Router.Navigate.Execute(_memoryOutputEditor);
                break;
            case CSV:
                HostScreen.Router.Navigate.Execute(_csvOutputEditor);
                break;
        }
    }
    
    public void ModifyOutput(IHaveOutputOptions selectedOutput)
    {
        switch (selectedOutput)
        {
            case ToMemoryStateOptions:
            {
                var vm = new MemoryOutputEditorViewModel(this, HostScreen);
                HostScreen.Router.Navigate.Execute(vm);
                break;
            }
            case ToCSVFileOptions:
            {
                var vm = new CsvOutputEditorViewModel(this, HostScreen);
                HostScreen.Router.Navigate.Execute(vm);
                break;
            }
        }
    }
    
    public void OnSaveOutput(IHaveOutputOptions newOptions)
    {
        _docEditor.UpdateOutput(newOptions);//.Outputs.Add;
        HostScreen.Router.Navigate.Execute(_docEditor);
    }

    public string? UrlPathSegment { get; } = "outputEditor";
    public IScreen HostScreen { get; }
}