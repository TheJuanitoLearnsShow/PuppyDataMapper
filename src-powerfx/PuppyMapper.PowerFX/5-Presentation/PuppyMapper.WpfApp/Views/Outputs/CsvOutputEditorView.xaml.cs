using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.ViewModels.Outputs;

namespace PuppyMapper.WpfApp.Views.Outputs;

public partial class CsvOutputEditorView : CsvOutputEditorViewBase
{
    public CsvOutputEditorView()
    {
        InitializeComponent();
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.Bind<CsvOutputEditorViewModel, CsvOutputEditorView, string, string?>(ViewModel, vm => vm.OutputId,
                    v => v.OutputId.Text)
                .DisposeWith(disposables);

            this.Bind<CsvOutputEditorViewModel, CsvOutputEditorView, string, string?>(ViewModel, vm => vm.FilePath,
                    v => v.FilePath.Text)
                .DisposeWith(disposables);

            this.BindCommand<CsvOutputEditorView, CsvOutputEditorViewModel, ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit>, Button>(ViewModel, vm => vm.SaveOutputCommand,
                v => v.SaveOutputBtn);

        });
    }
}
