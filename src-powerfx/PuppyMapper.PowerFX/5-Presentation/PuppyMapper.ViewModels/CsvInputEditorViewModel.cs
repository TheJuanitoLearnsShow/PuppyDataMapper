using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service.Integration;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PuppyMapper.Viewmodels;

public partial class CsvInputEditorViewModel : ReactiveObject
{
    [Reactive] private string _filePath = "Samples/CSV/SampleInput.csv";
    [Reactive] private string _inputId = "CsvInput";
    
    public IHaveInputOptions CreateInputOptions()
    {
        return new FromCSVFileOptions() { FilePath = _filePath, InputId = _inputId };
    }
}