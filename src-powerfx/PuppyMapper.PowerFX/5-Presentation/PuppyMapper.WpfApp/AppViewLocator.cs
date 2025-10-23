using System;
using PuppyMapper.WPFApp.Views;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.WPFApp;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        MappingDocumentIdeEditorViewModel context => new MappingDocumentIDEEditorView { DataContext = context },
        InputEditorViewModel context => new InputEditorView { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}