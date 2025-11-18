using System.Reactive;
using PuppyMapper.Viewmodels;
using ReactiveUI;

namespace PuppyMapper.WPFApp.ViewModels;

public partial class MainWindowViewModel : IScreen
{
    private readonly MappingDocumentIdeEditorViewModel _mappingDocEditor;

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new();

    // The command that navigates a user to first view model.
    public ReactiveCommand<Unit, IRoutableViewModel> GoMappingDocument { get; }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

    public MainWindowViewModel()
    {
        _mappingDocEditor = new MappingDocumentIdeEditorViewModel(this);
        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //
        GoMappingDocument = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(_mappingDocEditor)
        );
    }
}