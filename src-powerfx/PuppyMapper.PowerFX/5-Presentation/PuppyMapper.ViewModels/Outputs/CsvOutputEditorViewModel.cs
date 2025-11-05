using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.ViewModels.Outputs;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.ViewModels.Outputs;

public partial class CsvOutputEditorViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly OutputEditorViewModel _outputEditorViewModel;
    [Reactive] private string _filePath = "Samples/CSV/SampleOutput.csv";
    [Reactive] private string _outputId = "CsvOutput";

    public CsvOutputEditorViewModel(OutputEditorViewModel outputEditorViewModel, IScreen hostScreen)
    {
        _outputEditorViewModel = outputEditorViewModel;
        HostScreen = hostScreen;
    }

    private IHaveOutputOptions CreateOutputOptions()
    {
        return new ToCSVFileOptions() { FilePath = _filePath, OutputId = _outputId };
    }
    public string? UrlPathSegment { get; } = "CsvOutputEditor";
    public IScreen HostScreen { get; }
    
    [ReactiveCommand]
    private void SaveOutput()
    {
        var newOptions = CreateOutputOptions();
        _outputEditorViewModel.OnSaveOutput(newOptions);
    }
}