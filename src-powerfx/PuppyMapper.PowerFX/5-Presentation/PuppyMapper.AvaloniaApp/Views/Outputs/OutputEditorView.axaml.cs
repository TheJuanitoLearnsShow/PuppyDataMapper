using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using PuppyMapper.Viewmodels;
using PuppyMapper.ViewModels.Outputs;
using ReactiveUI;
using System.Reactive.Disposables.Fluent;

namespace PuppyMapper.AvaloniaApp.Views.Outputs;

public partial class OutputEditorView : ReactiveUserControl<OutputEditorViewModel>
{
    public OutputEditorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            
            this.OneWayBind<OutputEditorViewModel, OutputEditorView, ObservableCollection<string>, IEnumerable>(ViewModel, vm => vm.OutputTypes,
                    v => v.OutputTypes.ItemsSource)
                .DisposeWith(disposables);
            this.Bind<OutputEditorViewModel, OutputEditorView, string, object>(ViewModel, vm => vm.SelectedOutputType,
                    v => v.OutputTypes.SelectedItem)
                .DisposeWith(disposables);
            
            this.BindCommand<OutputEditorView, OutputEditorViewModel, ReactiveCommand<Unit, Unit>, Button>(ViewModel, vm => vm.AddOutputCommand,
                v => v.CreateOutputBtn);
            
        });
    }
}
