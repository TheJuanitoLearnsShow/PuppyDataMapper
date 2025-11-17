using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.ViewModels.Inputs;

namespace PuppyMapper.WpfApp.Views.Inputs;

public partial class InputEditorView : InputEditorViewBase
{
    public InputEditorView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.OneWayBind(ViewModel, vm => vm.InputTypes, v => v.InputTypes.ItemsSource)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.SelectedInputType, v => v.InputTypes.SelectedItem)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddInputCommand, v => v.CreateInputBtn)
                .DisposeWith(disposables);
        });
    }
}
