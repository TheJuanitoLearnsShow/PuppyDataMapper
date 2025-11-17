using System.Reactive.Threading.Tasks;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.Viewmodels;
using Xunit.Abstractions;

namespace PuppyMapper.PowerFX.Tests;

public class CsvIOTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CsvIOTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task TestFullMapping()
    {
        var ide = new MappingDocumentIdeEditorViewModel
        {
            MappingBaseFolderPath = "Samples/Xml/SampleCsvMapping.xml"
        };
        await ide.LoadMappingCommand.Execute().ToTask();
        await ide.ExecuteFullMappingCommand.Execute().ToTask();
        
        await File.WriteAllTextAsync("output-full-mapping-csv-io.json", ide.OutputData);
    }
}