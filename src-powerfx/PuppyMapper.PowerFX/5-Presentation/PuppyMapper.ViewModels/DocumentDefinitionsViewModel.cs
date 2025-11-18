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
    [Reactive] private string _mappingBaseFolderPath = string.Empty;
    
    public DocumentDefinitionsViewModel(IScreen hostScreen,
        Action<MappingDocumentIdeEditorViewModel> openDocumentAction,
        string mappingBaseFolderPath)
    {
        _hostScreen = hostScreen;
        _openDocumentAction = openDocumentAction;
        MappingBaseFolderPath = mappingBaseFolderPath;
    }
    [ReactiveCommand]
    private void AddDocument()
    {
        Documents.Add(new MappingDocumentIdeEditorViewModel(_hostScreen, MappingBaseFolderPath));
    }
    [ReactiveCommand]
    private void EditDocument()
    {
        if (_selectedDocument != null)
            _openDocumentAction(_selectedDocument);
    }
}