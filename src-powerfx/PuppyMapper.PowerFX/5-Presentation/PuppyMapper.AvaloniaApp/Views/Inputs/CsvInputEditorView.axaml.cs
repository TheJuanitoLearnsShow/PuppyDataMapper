using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using CsvInputEditorViewModel = PuppyMapper.ViewModels.Inputs.CsvInputEditorViewModel;

namespace PuppyMapper.AvaloniaApp.Views;

public partial class CsvInputEditorView : ReactiveUserControl<CsvInputEditorViewModel>
{
    public CsvInputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.Bind(ViewModel, vm => vm.InputId,
                    v => v.InputId.Text)
                .DisposeWith(disposables);
            
            
            this.Bind(ViewModel, vm => vm.FilePath,
                    v => v.FilePath.Text)
                .DisposeWith(disposables);
            
            this.BindCommand(ViewModel, vm => vm.SaveInputCommand,
                v => v.SaveInputBtn);
            
        });
    }
}