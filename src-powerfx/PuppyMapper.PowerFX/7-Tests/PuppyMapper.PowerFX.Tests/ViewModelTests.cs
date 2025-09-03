using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.XmlParser;
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
    
    
    [Fact]
    public void TestSerialization()
    {
        var doc = new MappingDocumentEditDto
        {
            MappingRulesCode = """
                               Diff := input.Score2 - baseSalary
                               Name2 := input.Name & "-suffix" // here is amultiline comments
                                                // I can put as many comments in here, next mapping line is the one that has the ":=" symbol 
                               Total := input.Score2 + input.Score
                               RowDiff := input.Score2 - input.Score // here acomments
                               """,
            InternalVarsCode = """
                               baseSalary := 561 * input.Score
                               """,
            MappingInputs = [
                new MappingInput("input", "Exam")
            ]
        };
        Assert.Equal(4, doc.MappingRules.Rules.Length);
        var xml = MappingDocumentXml.SerializeToXml(doc);
        File.WriteAllText("IDESavedMap.xml", xml);
        var deserializedDoc = MappingDocumentXml.DeserializeFromXml(xml);
        Assert.Equal(4, deserializedDoc.MappingRules.Rules.Length);
        Assert.Equal(1, deserializedDoc.InternalVars.Rules.Length);
        
    }
}