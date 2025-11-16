using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class DocumentDefinitionsViewModel : ReactiveObject
{
    private readonly IScreen _hostScreen;
    private readonly Action<MappingDocumentIdeEditorViewModel> _openDocumentAction;
    public ObservableCollection<MappingDocumentIdeEditorViewModel> Documents { get; set; } = [];
    [Reactive] private MappingDocumentIdeEditorViewModel? _selectedDocument;
    
    public DocumentDefinitionsViewModel(IScreen hostScreen, Action<MappingDocumentIdeEditorViewModel> openDocumentAction)
    {
        _hostScreen = hostScreen;
        _openDocumentAction = openDocumentAction;
    }
    [ReactiveCommand]
    private void AddDocument()
    {
        Documents.Add(new MappingDocumentIdeEditorViewModel(_hostScreen));
    }
}