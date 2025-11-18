// ...existing code...
using System;
using System.Reactive;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.WpfApp.ViewModels.Docking;

public class DocumentViewModel
{
    public DocumentViewModel(IScreen host, 
        MappingDocumentIdeEditorViewModel documentToOpen) 
    {
        // In WPF version we don't yet perform immediate navigation; keep placeholder logic
        // Router.Navigate.Execute(documentToOpen);
    }

    public void InitNavigation(
        IRoutableViewModel? document)
    {
        // navigation initialization (left for future use)
    }
}
