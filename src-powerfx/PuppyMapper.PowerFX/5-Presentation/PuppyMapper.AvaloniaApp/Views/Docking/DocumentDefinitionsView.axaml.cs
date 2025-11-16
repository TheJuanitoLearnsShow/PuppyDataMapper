using System.Reactive;
using System.Reactive.Disposables.Fluent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PuppyMapper.AvaloniaApp.Views.Inputs;
using PuppyMapper.Viewmodels;
using PuppyMapper.ViewModels.Inputs;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace PuppyMapper.AvaloniaApp.Views.Docking;

public partial class DocumentDefinitionsView : ReactiveUserControl<DocumentDefinitionsViewModel>
{
    public DocumentDefinitionsView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.BindCommand(ViewModel, vm => vm.AddDocumentCommand,
                v => v.AddMapping)
                .DisposeWith(disposables);;
            
        });
    }
}