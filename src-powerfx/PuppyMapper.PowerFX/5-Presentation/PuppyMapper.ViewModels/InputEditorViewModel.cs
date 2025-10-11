using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class InputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    public ObservableCollection<string> InputTypes { get; set; } = [];
    [Reactive] private string _selectedInputType = string.Empty;
    [Reactive] private CsvInputEditorViewModel _csvInputEditor = new();
    [Reactive] private MappingDocumentIdeEditorViewModel _docEditor;

    public InputEditorViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _docEditor = new(hostScreen);
    }
    public InputEditorViewModel(MappingDocumentIdeEditorViewModel docEditor, IScreen hostScreen)
    {
        _docEditor = docEditor;
        HostScreen = hostScreen;
    }
    
    [ReactiveCommand]
    private void AddInput()
    {
        var newInput = _csvInputEditor.CreateInputOptions();
        _docEditor.Inputs.Add(newInput);
    }

    public string? UrlPathSegment { get; } = "inputEditor";
    public IScreen HostScreen { get; }
}