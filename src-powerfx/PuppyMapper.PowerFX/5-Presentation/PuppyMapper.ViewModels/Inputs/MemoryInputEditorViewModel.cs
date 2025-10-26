using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.ViewModels.Inputs;

public partial class MemoryInputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly InputEditorViewModel _inputEditorViewModel;
    [Reactive] private string _propertyPath = "items";
    [Reactive] private string _inputId = "MemoryInput";

    public MemoryInputEditorViewModel(InputEditorViewModel inputEditorViewModel, IScreen hostScreen)
    {
        _inputEditorViewModel = inputEditorViewModel;
        HostScreen = hostScreen;
    }

    private IHaveInputOptions CreateInputOptions()
    {
        return new FromMemoryStateOptions() { PropertyPath = _propertyPath, InputId = _inputId };
    }
    public string? UrlPathSegment { get; } = "MemoryInputEditor";
    public IScreen HostScreen { get; }
    
    [ReactiveCommand]
    private void SaveInput()
    {
        var newOptions = CreateInputOptions();
        _inputEditorViewModel.OnSaveInput(newOptions);
    }
}