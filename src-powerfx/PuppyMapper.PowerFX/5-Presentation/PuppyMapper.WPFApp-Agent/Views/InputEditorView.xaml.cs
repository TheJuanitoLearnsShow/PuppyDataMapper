using System.Reactive.Disposables;
using System.Windows.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.WPFApp.Views;

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