using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Windows.Controls;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.WPFApp.Views;

public class MappingDocumentIDEEditorViewBase : ReactiveUserControl<MappingDocumentIdeEditorViewModel>
{
}

public partial class MappingDocumentIDEEditorView : MappingDocumentIDEEditorViewBase
{
    public MappingDocumentIDEEditorView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            // Update ViewModel when editor text changes
            this.WhenAnyValue(x => x.VarsCode.Text)
                .AddThrottleListener()
                .Subscribe<string>(text =>
                {
                    if (ViewModel != null && text != ViewModel.VarsCode)
                        ViewModel.VarsCode = text;
                })
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.VarsCode,
                    v => v.VarsCode.Text)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.VarsCode,
                    v => v.VarsCodeTxt.Text)
                .DisposeWith(disposables);

            this.WhenAnyValue(x => x.RulesCode.Text)
                .AddThrottleListener()
                .Subscribe<string>(text =>
                {
                    if (ViewModel != null && text != ViewModel.RulesCode)
                        ViewModel.RulesCode = text;
                })
                .DisposeWith(disposables);
        });
    }
}