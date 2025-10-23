using System.Collections.ObjectModel;
using PuppyMapper.PowerFX.Service.Integration;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class InputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    public ObservableCollection<string> InputTypes { get; set; } = ["CSV", "Memory"];
    [Reactive] private string _selectedInputType = string.Empty;
    [Reactive] private CsvInputEditorViewModel _csvInputEditor;
    [Reactive] private MappingDocumentIdeEditorViewModel _docEditor;

    public InputEditorViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _docEditor = new(hostScreen);
        _csvInputEditor = new(this, hostScreen);
    }
    public InputEditorViewModel(MappingDocumentIdeEditorViewModel docEditor, IScreen hostScreen)
    {
        _docEditor = docEditor;
        _csvInputEditor = new(this, hostScreen);
        HostScreen = hostScreen;
    }
    
    [ReactiveCommand]
    private void AddInput()
    {
        HostScreen.Router.Navigate.Execute(_csvInputEditor);
    }

    public void OnSaveInput(IHaveInputOptions newOptions)
    {
        _docEditor.Inputs.Add(newOptions);
        HostScreen.Router.Navigate.Execute(_docEditor);
    }

    public string? UrlPathSegment { get; } = "inputEditor";
    public IScreen HostScreen { get; }
}