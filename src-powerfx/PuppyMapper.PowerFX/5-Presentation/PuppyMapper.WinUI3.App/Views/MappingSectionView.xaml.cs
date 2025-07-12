using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public sealed partial class MappingSectionView : UserControl, IViewFor<MappingSectionViewModel>
{
    public MappingSectionView()
    {
        this.InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Name, v => v.SectionNameBox.Text)
                .DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.Rules, v => v.RulesList.ItemsSource)
                .DisposeWith(disposables);
        });
    }

    public MappingSectionViewModel ViewModel
    {
        get => (MappingSectionViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(MappingSectionViewModel), typeof(MappingSectionView), new PropertyMetadata(null));

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MappingSectionViewModel)value;
    }
}
