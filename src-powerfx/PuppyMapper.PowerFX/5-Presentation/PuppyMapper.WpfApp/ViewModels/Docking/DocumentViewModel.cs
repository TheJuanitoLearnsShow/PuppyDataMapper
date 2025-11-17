// ...existing code...
using System;
using System.Reactive;
using Dock.Model.ReactiveUI.Navigation.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.WpfApp.ViewModels.Docking;

public class DocumentViewModel
{

    public DocumentViewModel(IScreen host, MappingDocumentIdeEditorViewModel documentToOpen) : base(host)
    {
        Router.Navigate.Execute(documentToOpen);
        
    }

    public void InitNavigation(
        IRoutableViewModel? document)
    {
        // navigation initialization (left for future use)
    }
}

