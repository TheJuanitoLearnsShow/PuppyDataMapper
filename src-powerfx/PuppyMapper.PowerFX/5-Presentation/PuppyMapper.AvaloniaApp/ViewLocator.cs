using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Dock.Model.Core;
using PuppyMapper.AvaloniaApp.ViewModels.Docking;
using PuppyMapper.AvaloniaApp.Views.Docking;
using ReactiveUI;
using Splat;

namespace PuppyMapper.AvaloniaApp;

public partial class ViewLocator : IDataTemplate
{
    
    // Needed for Dock.Model to locate views for dockable view models
    public static void RegisterViews()
    {
        // Register new document views
        Locator.CurrentMutable.Register<IViewFor<DocumentViewModel>>(() => new DocumentView());
        Locator.CurrentMutable.Register<IViewFor<DocumentEditorViewModel>>(() => new DocumentEditorView());
    }
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        // Fallback to ReactiveUI's view locator for registered views
        var view = AppViewLocator.GetView(data);
        if (view is Control viewControl)
            return viewControl;
        var viewLocator = Locator.Current.GetService<IViewLocator>();
        if (viewLocator?.ResolveView(data) is Control control)
            return control;

        throw new Exception($"Unable to create view for type: {data.GetType()}");
    }

    public bool Match(object? data)
    {
        if (data is null)
        {
            return false;
        }

        if (data is IDockable)
        {
            return true;
        }

        var viewLocator = Locator.Current.GetService<IViewLocator>();
        return viewLocator?.ResolveView(data) is not null;
    }
}
