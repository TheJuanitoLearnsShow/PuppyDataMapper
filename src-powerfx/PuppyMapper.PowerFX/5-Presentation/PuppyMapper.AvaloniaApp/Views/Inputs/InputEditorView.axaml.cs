using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using InputEditorViewModel = PuppyMapper.ViewModels.Inputs.InputEditorViewModel;

namespace PuppyMapper.AvaloniaApp.Views;

public partial class InputEditorView : ReactiveUserControl<InputEditorViewModel>
{
    public InputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.OneWayBind(ViewModel, vm => vm.InputTypes,
                    v => v.InputTypes.ItemsSource)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedInputType,
                    v => v.InputTypes.SelectedItem)
                .DisposeWith(disposables);
            
            this.BindCommand(ViewModel, vm => vm.AddInputCommand,
                v => v.CreateInputBtn);
            
        });
    }
}