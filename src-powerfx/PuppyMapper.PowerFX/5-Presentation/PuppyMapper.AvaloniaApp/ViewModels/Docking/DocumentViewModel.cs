using System;
using System.Reactive;
using Dock.Model.ReactiveUI.Navigation.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.ViewModels.Docking;

public class DocumentViewModel : RoutableDocument
{

    public DocumentViewModel(IScreen host, MappingDocumentIdeEditorViewModel documentToOpen) : base(host)
    {
        Router.Navigate.Execute(documentToOpen);
        
    }

    public void InitNavigation(
        IRoutableViewModel? document)
    {
        // if (document is not null)
        // {
        //     GoDocument = ReactiveCommand.Create(() =>
        //         HostScreen.Router.Navigate.Execute(document).Subscribe(_ => { }));
        // }

    }
}
