using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PuppyMapper.AvaloniaApp.Views.Inputs;

public partial class InputTypeView : UserControl
{
    public InputTypeView()
    {
        InitializeComponent();
        // this.WhenActivated(disposables =>
        // {
        //     
        //     this.OneWayBind(ViewModel, vm => vm.InputTypes,
        //             v => v.InputTypes.ItemsSource)
        //         .DisposeWith(disposables);
        //     this.Bind(ViewModel, vm => vm.SelectedInputType,
        //             v => v.InputTypes.SelectedItem)
        //         .DisposeWith(disposables);
        //     
        //     this.BindCommand(ViewModel, vm => vm.AddInputCommand,
        //         v => v.CreateInputBtn);
        //     
        // });
    }
}