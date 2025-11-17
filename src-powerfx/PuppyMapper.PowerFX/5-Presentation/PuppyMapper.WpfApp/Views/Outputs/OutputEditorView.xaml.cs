using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.ViewModels.Outputs;

namespace PuppyMapper.WpfApp.Views.Outputs;

public partial class OutputEditorView : OutputEditorViewBase
{
    public OutputEditorView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.OneWayBind(ViewModel, vm => vm.OutputTypes, v => v.OutputTypes.ItemsSource)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.SelectedOutputType, v => v.OutputTypes.SelectedItem)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddOutputCommand, v => v.CreateOutputBtn)
                .DisposeWith(disposables);
        });
    }
}
