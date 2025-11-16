using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PuppyMapper.AvaloniaApp.ViewModels.Docking;
using ReactiveUI.Avalonia;

namespace PuppyMapper.AvaloniaApp.Views.Docking;

public partial class DocumentView : ReactiveUserControl<DocumentViewModel>
{
    public DocumentView()
    {
        InitializeComponent();
    }
}