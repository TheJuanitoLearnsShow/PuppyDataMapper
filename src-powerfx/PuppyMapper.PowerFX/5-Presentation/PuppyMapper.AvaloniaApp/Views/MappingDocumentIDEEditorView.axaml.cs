using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using ReactiveUI.Avalonia;
using PuppyMapper.AvaloniaApp.CodeEditor;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System.Reactive.Disposables.Fluent;

namespace PuppyMapper.AvaloniaApp.Views;
public class MappingDocumentIDEEditorViewBase : ReactiveUserControl<MappingDocumentIdeEditorViewModel>
{

}
public partial class MappingDocumentIDEEditorView : MappingDocumentIDEEditorViewBase
{
    public MappingDocumentIDEEditorView()
    {
        InitializeComponent();
        var rulesCodeSyntaxHighlighting = PowerFxHighlighting.Create();
        VarsCode.SyntaxHighlighting = rulesCodeSyntaxHighlighting;
        RulesCode.SyntaxHighlighting = rulesCodeSyntaxHighlighting;

        // Attach simple autocomplete suggestions to VarsCode
        CompletionSupport.AttachCompletion(VarsCode, new[]
        {
            "If", "Switch", "With", "Let", "As", "ThisRecord", "Self", "Parent", "true", "false",
            "Sum", "Filter", "LookUp", "Patch", "Collect", "Clear", "UpdateContext", "Navigate", "Rand", "Text", "Value"
        });

        this.WhenActivated(disposables =>
        {

            this.OneWayBind(ViewModel, vm => vm.VarsCode,
                    v => v.VarsCode.Text)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.RulesCode,
                    v => v.RulesCode.Text)
                .DisposeWith(disposables);



            this.OneWayBind(ViewModel, vm => vm.InputData,
                    v => v.InputTxt.Text)
                .DisposeWith(disposables);

            // Update ViewModel when editor text changes


            this.OneWayBind(ViewModel, vm => vm.OutputData,
                    v => v.OutputTxt.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.ExecuteMappingCommand,
                v => v.RunMappingBtn);
            this.BindCommand(ViewModel, vm => vm.LoadMappingCommand,
                v => v.LoadMappingBtn);

            this.BindCommand(ViewModel, vm => vm.ExecuteFullMappingCommand,
                v => v.RunFullMappingBtn);

            this.BindCommand(ViewModel, vm => vm.AddInputCommand,
                v => v.AddInputBtn);
            this.BindCommand(ViewModel, vm => vm.ModifyInputCommand,
                v => v.ModifyInputBtn);
            this.OneWayBind(ViewModel, vm => vm.Inputs,
                    v => v.Inputs.ItemsSource)
                .DisposeWith(disposables);
            
            this.BindCommand(ViewModel, vm => vm.AddOutputCommand,
                v => v.AddOutputBtn);
            this.BindCommand(ViewModel, vm => vm.ModifyOutputCommand,
                v => v.ModifyOutputBtn);
            this.OneWayBind(ViewModel, vm => vm.Outputs,
                    v => v.Outputs.ItemsSource)
                .DisposeWith(disposables);
            
            SetupCodeEditorListeners(disposables);

            ViewModel!.MappingFilePath = "Samples/Xml/SampleFxMapping.xml";

        });
    }

    private void SetupCodeEditorListeners(CompositeDisposable disposables)
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
        this.WhenAnyValue(x => x.RulesCode.Text)
            .AddThrottleListener()
            .Subscribe<string>(text =>
            {
                if (ViewModel != null && text != ViewModel.RulesCode)
                    ViewModel.RulesCode = text;
            })
            .DisposeWith(disposables);

        InputTxt.TextChanged += (s, e) =>
        {
            if (ViewModel != null && InputTxt.Text != ViewModel.InputData)
                ViewModel.InputData = InputTxt.Text;
        };

        this.WhenAnyValue(x => x.InputTxt.Text)
            .AddThrottleListener()
            .Subscribe<string>(text =>
            {
                if (ViewModel != null && text != ViewModel.InputData)
                    ViewModel.InputData = text;
            })
            .DisposeWith(disposables);
        this.WhenAnyValue(x => x.OutputTxt.Text)
            .AddThrottleListener()
            .Subscribe<string>(text =>
            {
                if (ViewModel != null && text != ViewModel.OutputData)
                    ViewModel.OutputData = text;
            })
            .DisposeWith(disposables);
    }
}