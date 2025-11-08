using System.Reactive;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PuppyMapper.AvaloniaApp.Views.Outputs;
using PuppyMapper.ViewModels.Outputs;
using PuppyMapper.ViewModels.Outputs;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.Views.Outputs;

public partial class CsvOutputEditorView : ReactiveUserControl<CsvOutputEditorViewModel>
{
    
    public CsvOutputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.Bind<CsvOutputEditorViewModel, CsvOutputEditorView, string, string?>(ViewModel, vm => vm.OutputId,
                    v => v.OutputId.Text)
                .DisposeWith(disposables);
            
            
            this.Bind<CsvOutputEditorViewModel, CsvOutputEditorView, string, string?>(ViewModel, vm => vm.FilePath,
                    v => v.FilePath.Text)
                .DisposeWith(disposables);
            
            this.BindCommand<CsvOutputEditorView, CsvOutputEditorViewModel, ReactiveCommand<Unit, Unit>, Button>(ViewModel, vm => vm.SaveOutputCommand,
                v => v.SaveOutputBtn);
            
        });
    }
}
