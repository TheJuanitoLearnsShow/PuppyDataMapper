using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public sealed partial class MappingRuleView : UserControl, IViewFor<MappingRuleViewModel>
{
    public MappingRuleView()
    {
        this.InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Name, v => v.RuleNameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Formula, v => v.FormulaBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Comments, v => v.CommentsBox.Text)
                .DisposeWith(disposables);
        });
    }

    public MappingRuleViewModel ViewModel
    {
        get => (MappingRuleViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(MappingRuleViewModel), typeof(MappingRuleView), new PropertyMetadata(null));

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MappingRuleViewModel)value;
    }
}
