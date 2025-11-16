using System;
using System.Reactive;
using Dock.Model.ReactiveUI.Navigation.Controls;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.ViewModels.Docking;

public class DocumentViewModel : RoutableDocument
{
    public ReactiveCommand<Unit, IDisposable>? GoDocument { get; private set; }
    public ReactiveCommand<Unit, IDisposable>? GoToEditor { get; private set; }

    public DocumentViewModel(IScreen host) : base(host)
    {
        // Router.Navigate.Execute(new DocumentHomeViewModel(this));
        
        GoToEditor = ReactiveCommand.Create(() =>
            Router.Navigate.Execute(new DocumentEditorViewModel(this, "Document Editor")).Subscribe(_ => { }));
    }

    public void InitNavigation(
        IRoutableViewModel? document)
    {
        if (document is not null)
        {
            GoDocument = ReactiveCommand.Create(() =>
                HostScreen.Router.Navigate.Execute(document).Subscribe(_ => { }));
        }

    }
}
