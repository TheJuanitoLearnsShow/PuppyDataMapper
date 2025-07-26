using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public class MappingInputViewBase : ReactiveUserControl<MappingInputViewModel>
{

}

public sealed partial class MappingInputView
{
    public MappingInputView()
    {
        this.InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.InputName, v => v.InputNameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.InputType, v => v.InputTypeBox.Text)
                .DisposeWith(disposables);
        });
    }

    public MappingInputViewModel ViewModel
    {
        get => (MappingInputViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}
