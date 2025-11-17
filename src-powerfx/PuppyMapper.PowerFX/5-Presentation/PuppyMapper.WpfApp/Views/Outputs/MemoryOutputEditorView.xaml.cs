using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.ViewModels.Outputs;

namespace PuppyMapper.WpfApp.Views.Outputs;

public partial class MemoryOutputEditorView : MemoryOutputEditorViewBase
{
    public MemoryOutputEditorView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.Bind(ViewModel, vm => vm.OutputId, v => v.OutputId.Text)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.PropertyPath, v => v.PropertyPath.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.SaveOutputCommand, v => v.SaveOutputBtn)
                .DisposeWith(disposables);
        });
    }
}
