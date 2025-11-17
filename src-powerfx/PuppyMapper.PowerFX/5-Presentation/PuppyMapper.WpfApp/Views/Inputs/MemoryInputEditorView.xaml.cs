using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.ViewModels.Inputs;

namespace PuppyMapper.WpfApp.Views.Inputs;

public partial class MemoryInputEditorView : MemoryInputEditorViewBase
{
    public MemoryInputEditorView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.Bind(ViewModel, vm => vm.InputId, v => v.InputId.Text)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.PropertyPath, v => v.PropertyPath.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.SaveInputCommand, v => v.SaveInputBtn)
                .DisposeWith(disposables);
        });
    }
}
