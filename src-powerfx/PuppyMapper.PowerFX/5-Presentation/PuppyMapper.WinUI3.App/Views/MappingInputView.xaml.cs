using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public sealed partial class MappingInputView : UserControl, IViewFor<MappingInputViewModel>
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

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(MappingInputViewModel), typeof(MappingInputView), new PropertyMetadata(null));

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MappingInputViewModel)value;
    }
}
