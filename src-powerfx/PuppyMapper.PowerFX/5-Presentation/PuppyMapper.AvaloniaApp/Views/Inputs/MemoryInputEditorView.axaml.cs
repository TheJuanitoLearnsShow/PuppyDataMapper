using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PuppyMapper.ViewModels.Inputs;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.Views.Inputs;

public partial class MemoryInputEditorView :  ReactiveUserControl<MemoryInputEditorViewModel>
{
    public MemoryInputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
                
            this.Bind(ViewModel, vm => vm.InputId,
                    v => v.InputId.Text)
                .DisposeWith(disposables);
                
                
            this.Bind(ViewModel, vm => vm.PropertyPath,
                    v => v.PropertyPath.Text)
                .DisposeWith(disposables);
                
            this.BindCommand(ViewModel, vm => vm.SaveInputCommand,
                v => v.SaveInputBtn);
                
        });
    }
}