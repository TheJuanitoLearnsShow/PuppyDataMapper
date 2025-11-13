using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using PuppyMapper.ViewModels.Outputs;
using ReactiveUI;
using System.Reactive.Disposables.Fluent;

namespace PuppyMapper.AvaloniaApp.Views.Outputs;

public partial class MemoryOutputEditorView : ReactiveUserControl<MemoryOutputEditorViewModel>
{
    public MemoryOutputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.Bind(ViewModel, vm => vm.OutputId,
                    v => v.OutputId.Text)
                .DisposeWith(disposables);
            
            
            this.Bind(ViewModel, vm => vm.PropertyPath,
                    v => v.PropertyPath.Text)
                .DisposeWith(disposables);
            
            this.BindCommand(ViewModel, vm => vm.SaveOutputCommand,
                v => v.SaveOutputBtn);
            
        });
    }
}
