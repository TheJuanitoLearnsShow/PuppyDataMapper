using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using PuppyMapper.Viewmodels;
using ReactiveUI;
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
            InputData = File.ReadAllText("Samples/SampleRecord1.json"),
            MappingFilePath = "Samples/Xml/SampleFxMapping.xml"
        };
        var observable = ide.WhenAnyValue(x => x.VarsCode)
            .Subscribe((string text) =>
            {
                _testOutputHelper.WriteLine(text);
            });
        ide.LoadMappingCommand.Execute().Subscribe();
        ide.ExecuteMappingCommand.Execute().Subscribe();
        _testOutputHelper.WriteLine(ide.OutputData);
        Assert.NotEmpty(ide.OutputData);
        ide.VarsCode = "test";
    }
    
    [Fact]
    public void TestMappingFromCSVFile()
    {
        var ide = new MappingDocumentIdeEditorViewModel
        {
            InputData = File.ReadAllText("Samples/SampleRecord1.json"),
            MappingFilePath = "Samples/Xml/SampleFxMapping.xml"
        };
        var observable = ide.WhenAnyValue(x => x.VarsCode)
            .Subscribe((string text) =>
            {
                _testOutputHelper.WriteLine(text);
            });
        ide.LoadMappingCommand.Execute().Subscribe();
        ide.ExecuteMappingCommand.Execute().Subscribe();
        _testOutputHelper.WriteLine(ide.OutputData);
        Assert.NotEmpty(ide.OutputData);
        ide.VarsCode = "test";
    }
}