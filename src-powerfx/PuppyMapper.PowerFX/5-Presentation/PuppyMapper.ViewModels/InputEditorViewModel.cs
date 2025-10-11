using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class InputEditorViewModel : ReactiveObject
{
    public ObservableCollection<string> InputTypes { get; set; } = [];
    [Reactive] private string _selectedInputType = string.Empty;
    [Reactive] private CsvInputEditorViewModel _csvInputEditor = new();
    [Reactive] private MappingDocumentIdeEditorViewModel _docEditor;

    public InputEditorViewModel()
    {
        _docEditor = new();
    }
    public InputEditorViewModel(MappingDocumentIdeEditorViewModel docEditor)
    {
        _docEditor = docEditor;
    }
    [ReactiveCommand]
    private void AddInput()
    {
        var newInput = _csvInputEditor.CreateInputOptions();
        _docEditor.Inputs.Add(newInput);
    }
}