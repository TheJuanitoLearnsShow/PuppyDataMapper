using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PuppyMapper.AvaloniaApp.ViewModels.Docking;
using ReactiveUI.Avalonia;

namespace PuppyMapper.AvaloniaApp.Views.Docking;

public partial class DocumentEditorView : ReactiveUserControl<DocumentEditorViewModel>
{
    public DocumentEditorView()
    {
        InitializeComponent();
    }
}