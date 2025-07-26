using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public class MappingSectionViewBase : ReactiveUserControl<MappingSectionViewModel>
{

}
public sealed partial class MappingSectionView 
{
    public MappingSectionView()
    {
        this.InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Name, v => v.SectionNameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Rules, v => v.RulesList.ItemsSource)
                .DisposeWith(disposables);
        });
    }
}
