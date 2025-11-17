using System;
using PuppyMapper.WpfApp.Views;
using PuppyMapper.WPFApp.Views; // mapping view lives in this namespace (note uppercase WPFApp)
using PuppyMapper.Viewmodels;
using ReactiveUI;
using PuppyMapper.WpfApp.Views.Docking;
using PuppyMapper.WpfApp.ViewModels.Docking;
using InputEditorViewModel = PuppyMapper.ViewModels.Inputs.InputEditorViewModel;

namespace PuppyMapper.WpfApp
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T? viewModel, string? contract = null)
        {
            return viewModel switch
            {
                MappingDocumentIdeEditorViewModel context => (IViewFor)new MappingDocumentIDEEditorView { DataContext = context },
                InputEditorViewModel context => (IViewFor)new InputEditorView { DataContext = context },
                DockingHostViewModel context => (IViewFor)new DockingHostView { DataContext = context },
                PuppyMapper.WpfApp.ViewModels.Docking.DocumentViewModel context => (IViewFor)new DocumentView { DataContext = context },
                PuppyMapper.WpfApp.ViewModels.Docking.DocumentEditorViewModel context => (IViewFor)new DocumentEditorView { DataContext = context },
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
            };
        }
    }
}
