using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PuppyMapper.Viewmodels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PuppyMapper.WinUI3.App.Views;

public class MappingEditorViewBase : ReactiveUserControl<MappingEditorViewModel>
{

}
public sealed partial class MappingEditorView : MappingEditorViewBase
{
    public MappingEditorView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.OneWayBind(ViewModel, vm => vm.MappingDocument, v => v.MappingDocumentView.ViewModel)
                .DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.InputData, v => v.InputTxt)
                .DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.OutputData, v => v.OutputTxt)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.ExecuteMappingCommand, v => v.RunMappingBtn);

            

        });
    }
}
