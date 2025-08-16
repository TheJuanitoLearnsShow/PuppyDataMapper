using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
            this.Bind(ViewModel, vm => vm.VarsCode, 
                    v => v.VarsCode.Text)
                ;
            this.Bind(ViewModel, vm => vm.RulesCode, 
                    v => v.RulesCode.Text)
                ;
            this.Bind(ViewModel, vm => vm.InputData, 
                    v => v.InputTxt.Text)
                ;
            this.OneWayBind(ViewModel, vm => vm.OutputData, 
                    v => v.OutputTxt.Text)
                ;
            this.BindCommand(ViewModel, vm => vm.ExecuteMappingCommand, 
                v => v.RunMappingBtn);
            this.BindCommand(ViewModel, vm => vm.LoadMappingCommand, 
                v => v.LoadMappingBtn, 
                vm => vm.MappingFilePath);

            
            ViewModel!.MappingFilePath = "Samples/Xml/SampleFxMapping.xml";

        });
    }
}