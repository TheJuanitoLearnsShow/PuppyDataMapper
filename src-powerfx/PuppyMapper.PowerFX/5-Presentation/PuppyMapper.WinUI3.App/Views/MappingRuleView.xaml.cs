using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public class MappingRuleViewBase : ReactiveUserControl<MappingRuleViewModel>
{

}
public sealed partial class MappingRuleView
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

}
