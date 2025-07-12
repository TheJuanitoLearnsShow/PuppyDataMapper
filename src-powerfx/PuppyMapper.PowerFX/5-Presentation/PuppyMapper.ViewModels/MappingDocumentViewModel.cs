using ReactiveUI;
using System.Collections.ObjectModel;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;
/*
 * In "PuppyMapper.WinUI.App\Views", add XAML user controls for the ReactiveUI views of the ViewModels in the PuppyManager.ViewModels project. In each of the View's code behind, add the ncessary ReactiveUI initialization code for each of the ViewModel properties.
 */
public partial class MappingDocumentViewModel : ReactiveObject
{
    [Reactive] public MappingSectionViewModel MappingRules { get; set; } = new();
    [Reactive] public MappingSectionViewModel InternalVars { get; set; } = new();
    [Reactive] public ObservableCollection<MappingInputViewModel> MappingInputs { get; set; } = new();
    // Add other properties as needed, e.g. output type, etc.
}
