using System;
using PuppyMapper.AvaloniaApp.Views;
using PuppyMapper.AvaloniaApp.Views.Inputs;
using PuppyMapper.Viewmodels;
using PuppyMapper.ViewModels.Inputs;
using ReactiveUI;
using CsvInputEditorViewModel = PuppyMapper.ViewModels.Inputs.CsvInputEditorViewModel;
using InputEditorViewModel = PuppyMapper.ViewModels.Inputs.InputEditorViewModel;

namespace PuppyMapper.AvaloniaApp;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        MappingDocumentIdeEditorViewModel context => new MappingDocumentIDEEditorView { DataContext = context },
        InputEditorViewModel context => new InputEditorView { DataContext = context },
        CsvInputEditorViewModel context => new CsvInputEditorView { DataContext = context },
        MemoryInputEditorViewModel context => new MemoryInputEditorView { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}