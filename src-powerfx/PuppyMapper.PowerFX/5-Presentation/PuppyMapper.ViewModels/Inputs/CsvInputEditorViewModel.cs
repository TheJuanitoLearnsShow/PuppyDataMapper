using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.ViewModels.Inputs;

public partial class CsvInputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly InputEditorViewModel _inputEditorViewModel;
    [Reactive] private string _filePath = "Samples/CSV/SampleInput.csv";
    [Reactive] private string _inputId = "CsvInput";

    public CsvInputEditorViewModel(InputEditorViewModel inputEditorViewModel, IScreen hostScreen)
    {
        _inputEditorViewModel = inputEditorViewModel;
        HostScreen = hostScreen;
    }

    private IHaveInputOptions CreateInputOptions()
    {
        return new FromCSVFileOptions() { FilePath = _filePath, InputId = _inputId };
    }
    public string? UrlPathSegment { get; } = "CsvInputEditor";
    public IScreen HostScreen { get; }
    
    [ReactiveCommand]
    private void SaveInput()
    {
        var newOptions = CreateInputOptions();
        _inputEditorViewModel.OnSaveInput(newOptions);
    }
}

public partial class CsvOutputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly InputEditorViewModel _inputEditorViewModel;
    [Reactive] private string _filePath = "Samples/CSV/SampleInput.csv";
    [Reactive] private string _inputId = "CsvInput";

    public CsvOutputEditorViewModel(OutputEditorViewModel inputEditorViewModel, IScreen hostScreen)
    {
        _inputEditorViewModel = inputEditorViewModel;
        HostScreen = hostScreen;
    }

    private IHaveInputOptions CreateInputOptions()
    {
        return new FromCSVFileOptions() { FilePath = _filePath, InputId = _inputId };
    }
    public string? UrlPathSegment { get; } = "CsvInputEditor";
    public IScreen HostScreen { get; }
    
    [ReactiveCommand]
    private void SaveInput()
    {
        var newOptions = CreateInputOptions();
        _inputEditorViewModel.OnSaveInput(newOptions);
    }
}