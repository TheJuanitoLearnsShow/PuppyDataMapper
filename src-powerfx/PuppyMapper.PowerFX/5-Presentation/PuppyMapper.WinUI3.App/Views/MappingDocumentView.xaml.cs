using Microsoft.UI.Xaml.Controls;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace PuppyMapper.WinUI3.App.Views;

public sealed partial class MappingDocumentView : UserControl, IViewFor<MappingDocumentViewModel>
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

    public MappingDocumentViewModel ViewModel
    {
        get => (MappingDocumentViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(MappingDocumentViewModel), typeof(MappingDocumentView), new PropertyMetadata(null));

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MappingDocumentViewModel)value;
    }
}
