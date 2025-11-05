using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.ViewModels.Outputs;

public partial class MemoryOutputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly OutputEditorViewModel _outputEditorViewModel;
    [Reactive] private string _propertyPath = "items";
    [Reactive] private string _outputId = "MemoryOutput";

    public MemoryOutputEditorViewModel(OutputEditorViewModel outputEditorViewModel, IScreen hostScreen)
    {
        _outputEditorViewModel = outputEditorViewModel;
        HostScreen = hostScreen;
    }

    private IHaveOutputOptions CreateOutputOptions()
    {
        return new ToMemoryStateOptions() { PropertyPath = _propertyPath, OutputId = _outputId };
    }
    public string? UrlPathSegment { get; } = "MemoryOutputEditor";
    public IScreen HostScreen { get; }
    
    [ReactiveCommand]
    private void SaveOutput()
    {
        var newOptions = CreateOutputOptions();
        _outputEditorViewModel.OnSaveOutput(newOptions);
    }
}