using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using Dock.Model.ReactiveUI.Navigation.Controls;
using ReactiveUI;

namespace PuppyMapper.AvaloniaApp.ViewModels.Docking;

public class DockFactory : Factory
{
    private readonly IScreen _host;
    private DocumentDock _documentDock;
    private RoutableRootDock _root;

    public DockFactory(IScreen host)
    {
        _host = host;
    }

    public void ShowDocument(DocumentViewModel openedDocument)
    {
        _documentDock.AddDocument(openedDocument);
        // _documentDock.VisibleDockables?.Add(openedDocument);
        // _documentDock.ActiveDockable = openedDocument;
        // _root.VisibleDockables?.Add(openedDocument);
        // _root.ActiveDockable = openedDocument;
    }
    public override IRootDock CreateLayout()
    {
        // var document1 = new DocumentViewModel(_host) { Id = "Doc1", Title = "Document 1" };
        // var document2 = new DocumentViewModel(_host) { Id = "Doc2", Title = "Document 2" };
        // // var tool1 = new ToolViewModel(_host) { Id = "Tool1", Title = "Tool 1" };
        // // var tool2 = new ToolViewModel(_host) { Id = "Tool2", Title = "Tool 2" };
        //
        // document1.InitNavigation(document2);
        // document2.InitNavigation(document1);
        // tool1.InitNavigation(document1, document2, tool2);
        // tool2.InitNavigation(document1, document2, tool1);

        _documentDock = new DocumentDock
        {
            Id = "Documents",
            VisibleDockables = [],
            ActiveDockable = null,
            CanCreateDocument = false,
            
        };

        // var toolDock = new ToolDock
        // {
        //     Id = "Tools",
        //     VisibleDockables = CreateList<IDockable>(tool1, tool2),
        //     ActiveDockable = tool1,
        //     Alignment = Alignment.Left,
        //     Proportion = 0.25
        // };

        // var mainLayout = new ProportionalDock
        // {
        //     Orientation = Orientation.Horizontal,
        //     VisibleDockables = CreateList<IDockable>(new ProportionalDockSplitter(), documentDock),
        //     ActiveDockable = documentDock
        // };

        _root = new RoutableRootDock(_host)
        {
            VisibleDockables = CreateList<IDockable>(_documentDock),
            DefaultDockable = _documentDock,
            ActiveDockable = _documentDock,
            LeftPinnedDockables = CreateList<IDockable>(),
            RightPinnedDockables = CreateList<IDockable>(),
            TopPinnedDockables = CreateList<IDockable>(),
            BottomPinnedDockables = CreateList<IDockable>(),
            PinnedDock = null
        };

        return _root;
    }

    public override void InitLayout(IDockable layout)
    {
        HostWindowLocator = new Dictionary<string, Func<IHostWindow?>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };
        
        base.InitLayout(layout);
    }
}
