using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.JsonParser;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;

namespace PuppyMapper.Viewmodels;
/*
 * In "PuppyMapper.WinUI.App\Views", add XAML user controls for the ReactiveUI views of the ViewModels in the PuppyManager.ViewModels project. In each of the View's code behind, add the ncessary ReactiveUI initialization code for each of the ViewModel properties.
 */
public partial class MappingDocumentViewModel : ReactiveObject
{
    public MappingDocumentViewModel()
    {

    }
    public MappingDocumentViewModel(MappingDocumentDto doc)
    {
        MappingRules = new MappingSectionViewModel(doc.MappingRules);
        InternalVars = new MappingSectionViewModel(doc.InternalVars);
        MappingInputs = new ObservableCollection<MappingInputViewModel>(
            doc.MappingInputs.Select(input => new MappingInputViewModel(input)));
    }

    [Reactive] public MappingSectionViewModel MappingRules { get; set; } = new();
    [Reactive] public MappingSectionViewModel InternalVars { get; set; } = new();
    [Reactive] public ObservableCollection<MappingInputViewModel> MappingInputs { get; set; } = new();
    // Add other properties as needed, e.g. output type, etc.

    [ReactiveCommand]
    private async Task LoadDocument(string filePath)
    {
        var doc = await MappingDocumentJson.ParseDocument(filePath);
        LoadData(doc);
    }

    public void LoadData(MappingDocumentDto doc)
    {
        MappingRules = new MappingSectionViewModel(doc.MappingRules);
        InternalVars = new MappingSectionViewModel(doc.InternalVars);
        MappingInputs = new ObservableCollection<MappingInputViewModel>(
            doc.MappingInputs.Select(input => new MappingInputViewModel(input)));
    }
}
