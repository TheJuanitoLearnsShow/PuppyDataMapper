using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using OutputEditorViewModel = PuppyMapper.ViewModels.Outputs.OutputEditorViewModel;

namespace PuppyMapper.AvaloniaApp.Views;

public partial class OutputEditorView : ReactiveUserControl<OutputEditorViewModel>
{
    public OutputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.OneWayBind(ViewModel, vm => vm.OutputTypes,
                    v => v.OutputTypes.ItemsSource)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedOutputType,
                    v => v.OutputTypes.SelectedItem)
                .DisposeWith(disposables);
            
            this.BindCommand(ViewModel, vm => vm.AddOutputCommand,
                v => v.CreateOutputBtn);
        });
    }
}
