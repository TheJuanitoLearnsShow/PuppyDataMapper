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
            this.OneWayBind(ViewModel, vm => vm.MappingDocument, 
                    v => v.MappingDocument)
                ;
            this.OneWayBind(ViewModel, vm => vm.InputData, 
                    v => v.InputTxt)
                ;
            this.OneWayBind(ViewModel, vm => vm.OutputData, 
                    v => v.OutputTxt)
                ;
            this.BindCommand(ViewModel, vm => vm.ExecuteMappingCommand, 
                v => v.RunMappingBtn);

            

        });
    }
}