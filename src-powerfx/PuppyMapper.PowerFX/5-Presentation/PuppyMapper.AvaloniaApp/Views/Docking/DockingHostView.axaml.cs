using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PuppyMapper.AvaloniaApp.ViewModels;
using PuppyMapper.AvaloniaApp.ViewModels.Docking;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace PuppyMapper.AvaloniaApp.Views.Docking;

public partial class DockingHostView : ReactiveUserControl<DockingHostViewModel>
{
    public DockingHostView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            
            this.Bind(ViewModel, vm => vm.Help,
                v => v.HelpTxt.Text)
                .DisposeWith(disposables);;
            this.Bind(ViewModel, vm => vm.Layout,
                v => v.DockCtrl.Layout)
                .DisposeWith(disposables);;
            
        });
        // var list = this.FindControl<ListBox>("DocumentsList");
        // list.DoubleTapped += (s, e) =>
        // {
        //     if (DataContext is MainWindowViewModel vm && list.SelectedItem is MappingDocumentIdeEditorViewModel doc)
        //     {
        //         vm.OpenDocument(doc);
        //     }
        // };
    }

}
