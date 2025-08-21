using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaEdit.Utils;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.Views;
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
            VarsCode.TextChanged += (s, e) =>
            {
                if (ViewModel != null && VarsCode.Text != ViewModel.VarsCode)
                    ViewModel.VarsCode = VarsCode.Text;
            };

            // Update editor when ViewModel changes
            this.WhenAnyValue(x => x.ViewModel.VarsCode)
                .Subscribe((string text) =>
                {
                    if (VarsCode.Text != text)
                        VarsCode.Text = text ?? string.Empty;
                })
                .DisposeWith(disposables);
            
            // Update ViewModel when editor text changes
            RulesCode.TextChanged += (s, e) =>
            {
                if (ViewModel != null && RulesCode.Text != ViewModel.RulesCode)
                    ViewModel.RulesCode = RulesCode.Text;
            };

            // Update editor when ViewModel changes
            this.WhenAnyValue(x => x.ViewModel!.RulesCode)
                .Subscribe((string text) =>
                {
                    if (RulesCode.Text != text)
                        RulesCode.Text = text ?? string.Empty;
                })
                .DisposeWith(disposables);
            
            // Update ViewModel when editor text changes
            InputTxt.TextChanged += (s, e) =>
            {
                if (ViewModel != null && InputTxt.Text != ViewModel.InputData)
                    ViewModel.InputData = InputTxt.Text;
            };

            
            // Update editor when ViewModel changes
            this.WhenAnyValue(x => x.ViewModel!.InputData)
                .Subscribe((string text) =>
                {
                    if (InputTxt.Text != text)
                        InputTxt.Text = text ?? string.Empty;
                })
                .DisposeWith(disposables);
            
            
            // Update ViewModel when editor text changes
            OutputTxt.TextChanged += (s, e) =>
            {
                if (ViewModel != null && OutputTxt.Text != ViewModel.OutputData)
                    ViewModel.OutputData = OutputTxt.Text;
            };

            // Update editor when ViewModel changes
            this.WhenAnyValue(x => x.ViewModel!.OutputData)
                .Subscribe((string text) =>
                {
                    if (OutputTxt.Text != text)
                        OutputTxt.Text = text ?? string.Empty;
                })
                .DisposeWith(disposables);
            // this.Bind(ViewModel, vm => vm.VarsCode, 
            //         v => v.VarsCode.Text)
            //     ;
            // this.Bind(ViewModel, vm => vm.RulesCode, 
            //         v => v.RulesCode.Text)
            //     ;
            // this.Bind(ViewModel, vm => vm.InputData, 
            //         v => v.InputTxt.Text)
            //     ;
            // this.OneWayBind(ViewModel, vm => vm.OutputData, 
            //         v => v.OutputTxt.Text)
                ;
            this.BindCommand(ViewModel, vm => vm.ExecuteMappingCommand, 
                v => v.RunMappingBtn);
            this.BindCommand(ViewModel, vm => vm.LoadMappingCommand, 
                v => v.LoadMappingBtn);

            
            ViewModel!.MappingFilePath = "Samples/Xml/SampleFxMapping.xml";

        });
    }
}