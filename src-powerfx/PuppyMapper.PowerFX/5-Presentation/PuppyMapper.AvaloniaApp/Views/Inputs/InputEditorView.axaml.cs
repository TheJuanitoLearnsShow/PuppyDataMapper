using System.Collections;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using InputEditorViewModel = PuppyMapper.ViewModels.Inputs.InputEditorViewModel;

namespace PuppyMapper.AvaloniaApp.Views.Inputs;

public partial class InputEditorView : ReactiveUserControl<InputEditorViewModel>
{
    public InputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.OneWayBind<InputEditorViewModel, InputEditorView, ObservableCollection<string>, IEnumerable>(ViewModel, vm => vm.InputTypes,
                    v => v.InputTypes.ItemsSource)
                .DisposeWith(disposables);
            this.Bind<InputEditorViewModel, InputEditorView, string, object>(ViewModel, vm => vm.SelectedInputType,
                    v => v.InputTypes.SelectedItem)
                .DisposeWith(disposables);
            
            this.BindCommand<InputEditorView, InputEditorViewModel, ReactiveCommand<Unit, Unit>, Button>(ViewModel, vm => vm.AddInputCommand,
                v => v.CreateInputBtn);
            
        });
    }
}