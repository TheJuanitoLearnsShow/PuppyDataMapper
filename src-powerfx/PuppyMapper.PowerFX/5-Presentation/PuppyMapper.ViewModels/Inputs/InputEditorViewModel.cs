using System.Collections.ObjectModel;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.ViewModels.Inputs;



public partial class InputsSectionView : ReactiveObject, IRoutableViewModel
{
    [Reactive] private string _inputData = string.Empty;
    [Reactive] private MappingDocumentIdeEditorViewModel _docEditor;
    [Reactive] private InputEditorViewModel _inputEditor;
    
    public InputsSectionView(MappingDocumentIdeEditorViewModel docEditor, IScreen hostScreen)
    {
        _docEditor = docEditor;
        HostScreen = hostScreen;
    }
    public string? UrlPathSegment { get; } = "inputsSection";
    public IScreen HostScreen { get; }
}

public partial class InputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private const string Memory = "Memory";
    private const string CSV = "CSV";
    public ObservableCollection<string> InputTypes { get; set; } = [CSV, Memory];
    [Reactive] private string _selectedInputType = string.Empty;
    [Reactive] private CsvInputEditorViewModel _csvInputEditor;
    [Reactive] private MemoryInputEditorViewModel _memoryInputEditor;
    [Reactive] private MappingDocumentIdeEditorViewModel _docEditor;

    public InputEditorViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _docEditor = new(hostScreen);
        _csvInputEditor = new(this, hostScreen);
        _memoryInputEditor = new(this, hostScreen);
    }
    public InputEditorViewModel(MappingDocumentIdeEditorViewModel docEditor, IScreen hostScreen)
    {
        _docEditor = docEditor;
        _csvInputEditor = new(this, hostScreen);
        _memoryInputEditor = new(this, hostScreen);
        HostScreen = hostScreen;
    }
    
    [ReactiveCommand]
    private void AddInput()
    {
        switch (_selectedInputType)
        {
            case Memory:
                HostScreen.Router.Navigate.Execute(_memoryInputEditor);
                break;
            case CSV:
                HostScreen.Router.Navigate.Execute(_csvInputEditor);
                break;
        }
    }
    
    public void ModifyInput(IHaveInputOptions selectedInput)
    {
        switch (selectedInput)
        {
            case FromMemoryStateOptions:
            {
                var vm = new MemoryInputEditorViewModel(this, HostScreen);
                HostScreen.Router.Navigate.Execute(vm);
                break;
            }
            case FromCSVFileOptions:
            {
                var vm = new CsvInputEditorViewModel(this, HostScreen);
                HostScreen.Router.Navigate.Execute(vm);
                break;
            }
        }
    }
    
    public void OnSaveInput(IHaveInputOptions newOptions)
    {
        _docEditor.UpdateInput(newOptions);//.Inputs.Add;
        HostScreen.Router.Navigate.Execute(_docEditor);
    }

    public string? UrlPathSegment { get; } = "inputEditor";
    public IScreen HostScreen { get; }
}