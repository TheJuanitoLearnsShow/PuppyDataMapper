using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.Views;

public partial class InputEditorView : ReactiveUserControl<InputEditorViewModel>
{
    public InputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.SelectedInputType, 
                    v => v.SelectedInputType.Text)
                .DisposeWith(disposables);
        });
    }
}