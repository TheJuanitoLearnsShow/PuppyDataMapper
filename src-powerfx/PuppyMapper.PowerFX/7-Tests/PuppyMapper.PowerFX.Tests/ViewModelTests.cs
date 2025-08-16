using PuppyMapper.Viewmodels;
using Xunit.Abstractions;

namespace PuppyMapper.PowerFX.Tests;

public class ViewModelTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ViewModelTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestMappingViaViewModel()
    {
        var ide = new MappingDocumentIdeEditorViewModel
        {
            InputData = File.ReadAllText("Samples/SampleRecord1.json")
        };
        ide.LoadMappingCommand.Execute("Samples/Xml/SampleFxMapping.xml").Subscribe();
        ide.ExecuteMappingCommand.Execute().Subscribe();
        _testOutputHelper.WriteLine(ide.OutputData);
        Assert.NotEmpty(ide.OutputData);
    }
}