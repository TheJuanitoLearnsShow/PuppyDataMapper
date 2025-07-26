using Microsoft.UI.Xaml.Controls;
using PuppyMapper.PowerFX.Service.JsonParser;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public class MappingDocumentViewBase : ReactiveUserControl<MappingDocumentViewModel> 
{

}
public sealed partial class MappingDocumentView 
{
    public MappingDocumentView()
    {
        this.InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.OneWayBind(ViewModel, vm => vm.MappingRules, v => v.MappingRulesView.ViewModel)
                .DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.InternalVars, v => v.InternalVarsView.ViewModel)
                .DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.MappingInputs, v => v.MappingInputsList.ItemsSource)
                .DisposeWith(disposables);
        });
    }


}
