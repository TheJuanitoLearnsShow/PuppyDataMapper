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
    public void TestParseSections()
    {
        var fileContents = File.ReadAllText("Samples/SampleFxMappingViewModel.txt");

        var ide = new MappingDocumentIdeEditorViewModel
        {
            MappingDocument = fileContents,
            InputData = File.ReadAllText("Samples/SampleRecord1.json")
        };
        ide.ExecuteMappingCommand.Execute().Subscribe();
        _testOutputHelper.WriteLine(ide.OutputData);
        Assert.NotEmpty(ide.OutputData);
    }
}