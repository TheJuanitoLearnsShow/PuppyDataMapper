using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using CsvInputEditorViewModel = PuppyMapper.ViewModels.Inputs.CsvInputEditorViewModel;

namespace PuppyMapper.AvaloniaApp.Views.Inputs;

public partial class CsvInputEditorView : ReactiveUserControl<CsvInputEditorViewModel>
{
    public CsvInputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.Bind<CsvInputEditorViewModel, CsvInputEditorView, string, string?>(ViewModel, vm => vm.InputId,
                    v => v.InputId.Text)
                .DisposeWith(disposables);
            
            
            this.Bind<CsvInputEditorViewModel, CsvInputEditorView, string, string?>(ViewModel, vm => vm.FilePath,
                    v => v.FilePath.Text)
                .DisposeWith(disposables);
            
            this.BindCommand<CsvInputEditorView, CsvInputEditorViewModel, ReactiveCommand<Unit, Unit>, Button>(ViewModel, vm => vm.SaveInputCommand,
                v => v.SaveInputBtn);
            
        });
    }
}