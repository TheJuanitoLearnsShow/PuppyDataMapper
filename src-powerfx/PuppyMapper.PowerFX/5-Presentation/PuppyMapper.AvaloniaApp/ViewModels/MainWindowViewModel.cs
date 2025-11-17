using System;
using System.Reactive;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Collections.ObjectModel;
using PuppyMapper.Viewmodels;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using System.Linq;
using PuppyMapper.AvaloniaApp.ViewModels.Docking;

namespace PuppyMapper.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IScreen
{
    private readonly MappingDocumentIdeEditorViewModel _mappingDocEditor;

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new ();

    // The command that navigates a user to first view model.
    public ReactiveCommand<Unit, IRoutableViewModel> GoMappingDocument { get; }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

    // Collection of available mapping documents
    public ObservableCollection<MappingDocumentIdeEditorViewModel> Documents { get; } = new();

    // Collection of open documents shown as tabs
    public ObservableCollection<MappingDocumentIdeEditorViewModel> OpenDocuments { get; } = new();

    // Command to add a new mapping document
    public ReactiveCommand<Unit, Unit> AddDocumentCommand { get; }

    // Command to close a document
    public ReactiveCommand<MappingDocumentIdeEditorViewModel, Unit> CloseDocumentCommand { get; }

    private MappingDocumentIdeEditorViewModel? _selectedDocument;
    public MappingDocumentIdeEditorViewModel? SelectedDocument
    {
        get => _selectedDocument;
        set => SetProperty(ref _selectedDocument, value);
    }

    // Dock model root bound to DockHost
    public Dock.Model.Core.IDock? DockModel { get; private set; }

    public MainWindowViewModel()
    {
        _mappingDocEditor = new MappingDocumentIdeEditorViewModel(this);
        _mappingDocEditor.OutputData = "Test";

        // add a sample document to Documents
        Documents.Add(_mappingDocEditor);

        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //
        GoMappingDocument = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(_mappingDocEditor)
        );

        AddDocumentCommand = ReactiveCommand.Create(() =>
        {
            var doc = new MappingDocumentIdeEditorViewModel(this)
            {
                MappingBaseFolderPath = "NewMapping"
            };
            Documents.Add(doc);
            // OpenDocument(doc);
            SelectedDocument = doc;
        });

        // CloseDocumentCommand = ReactiveCommand.Create<MappingDocumentIdeEditorViewModel>(CloseDocument);

        // create Dock.Model root
        
        var dockViewModel = new DockingHostViewModel(this);
        Router.Navigate.Execute(dockViewModel).Subscribe();
    }

    // public void OpenDocument(MappingDocumentIdeEditorViewModel? doc)
    // {
    //     if (doc == null)
    //         return;
    //
    //     if (!OpenDocuments.Contains(doc))
    //         OpenDocuments.Add(doc);
    //
    //     SelectedDocument = doc;
    //
    //     // create a document dockable and add to DockModel document dock if available
    //     if (DockModel is Dock.Model.Core.IDock dockRoot)
    //     {
    //         var docDock = dockRoot.Find(isDocument: true);
    //         if (docDock != null)
    //         {
    //             var d = new DocumentViewModel
    //             {
    //                 Id = doc.MappingFilePath ?? System.Guid.NewGuid().ToString(),
    //                 Title = doc.MappingFilePath,
    //                 Content = doc,
    //                 IsActive = true,
    //                 CanClose = true
    //             };
    //
    //             // Hook up the close command so closing the tab removes the document from our collections
    //             try
    //             {
    //                 d.CloseCommand = ReactiveCommand.Create(() => CloseDocument(doc));
    //             }
    //             catch
    //             {
    //                 // some versions of Dock.Model expose CloseCommand as ICommand and may already be settable
    //             }
    //
    //             docDock.Children.Add(d);
    //             docDock.ActiveDockable = d;
    //         }
    //     }
    // }
    //
    // public void CloseDocument(MappingDocumentIdeEditorViewModel? doc)
    // {
    //     if (doc == null)
    //         return;
    //
    //     if (OpenDocuments.Contains(doc))
    //         OpenDocuments.Remove(doc);
    //
    //     if (SelectedDocument == doc)
    //         SelectedDocument = OpenDocuments.Count > 0 ? OpenDocuments[0] : null;
    //
    //     if (DockModel is Dock.Model.Core.IDock dockRoot)
    //     {
    //         var docDock = dockRoot.Find(isDocument: true);
    //         if (docDock != null)
    //         {
    //             var found = docDock.Children.FirstOrDefault(c => c.Id == doc.MappingFilePath);
    //             if (found != null)
    //             {
    //                 docDock.Children.Remove(found);
    //             }
    //         }
    //     }
    // }
}
