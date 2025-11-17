using System.Reactive.Threading.Tasks;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.Viewmodels;
using Xunit.Abstractions;

namespace PuppyMapper.PowerFX.Tests;

public class MemoryIOTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MemoryIOTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task TestFullMapping()
    {
        var ide = new MappingDocumentIdeEditorViewModel
        {
            MappingBaseFolderPath = "Samples/Xml/SampleMemoryMapping.xml"
        };
        var globalData = MemorySateManager.GetState();
        Dictionary<string, object>[] students = [
            new Dictionary<string, object>(){
                {"Score", 10},
                {"Score2", 10},
                {"Name", "Pepe"},
            },
            new Dictionary<string, object>(){
                {"Score", 20},
                {"Score2", 40},
                {"Name", "Juan"},
            },
        ];
        
        globalData.Add("Students", new Dictionary<string, object>()
            {
                {"Scores", students},
                {"Total", students.Length}
            }
        );
        await ide.LoadMappingCommand.Execute().ToTask();
        await ide.ExecuteFullMappingCommand.Execute().ToTask();
        
        await File.WriteAllTextAsync("output-full-mapping-memory-io.json", ide.OutputData);
    }
}