using Dock.Model.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.AvaloniaApp.ViewModels.Docking;

public partial class DockingHostViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly DockFactory _factory;
    [Reactive] private IRootDock? _layout;

    public string UrlPathSegment { get; } = "dock";
    public IScreen HostScreen { get; }
    
    [Reactive] private string _help;

    public DockingHostViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _factory = new DockFactory(hostScreen);
        var layout = _factory.CreateLayout();
        if (layout is not null)
        {
            _factory.InitLayout(layout);
        }
        Layout = layout;
        Help = "This is help";
    }
}