using System.Reactive.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using PuppyMapper.IntegrationProviders;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.Integration;
using PuppyMapper.PowerFX.Service.XmlParser;
using PuppyMapper.Viewmodels;
using ReactiveUI;
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
            MappingFilePath = "Samples/Xml/SampleMemoryMapping.xml"
        };
        var globalData = MemorySateManager.GetState();
        globalData
        await ide.LoadMappingCommand.Execute().ToTask();
        await ide.ExecuteFullMappingCommand.Execute().ToTask();
        
        await File.WriteAllTextAsync("output-full-mapping.json", ide.OutputData);
    }
}

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
    public void TestSerializationXml()
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
                new MappingInput("input", "ExamData")
            ]
        };
        var persistenceModel = new MappingPersistenceModel
        {
            Document = doc,
            CSVInputs = [
                new FromCSVFileOptions() { FilePath = "Samples/CSV/SampleInput.csv", InputId = "ExamData"}
            ],
            MemoryOutputs = [
                new ToMemoryStateOptions() { PropertyPath = "Output.ExamResults.[RowIndex]" }
            ]
        };
        Assert.Equal(4, doc.MappingRules.Rules.Length);
        var xml = MappingPersistenceModel.SerializeToXml(persistenceModel);
        File.WriteAllText("IDESavedMap.xml", xml);
        var deserializedDoc = MappingPersistenceModel.DeserializeFromXml(xml);
        Assert.Equal(4, deserializedDoc.Document.MappingRules.Rules.Length);
        Assert.Equal(1, deserializedDoc.Document.InternalVars.Rules.Length);
        
        
        
    }
    
    
    [Fact]
    public void TestSerializationViewModel()
    {
        var ide = new MappingDocumentIdeEditorViewModel
        {
            InputData = File.ReadAllText("Samples/SampleRecord1.json"),
            MappingFilePath = "Samples/Xml/SampleFxMapping.xml"
        };
        ide.LoadMappingCommand.Execute().Subscribe(_ =>
        {
            ide.MappingFilePath = "SavedFromVM.xml";
            ide.SaveMappingCommand.Execute().Subscribe();
        });

    }
    
    [Fact]
    public async Task TestFullMapping()
    {
        
        var ide = new MappingDocumentIdeEditorViewModel
        {
            // InputData = File.ReadAllText("Samples/SampleRecord1.json"),
            MappingFilePath = "Samples/Xml/SampleFxMapping.xml"
        };
        await ide.LoadMappingCommand.Execute().ToTask();
        await ide.ExecuteFullMappingCommand.Execute().ToTask();
        
        await File.WriteAllTextAsync("output-full-mapping.json", ide.OutputData);
    }
    
    private void OutputDictionary(ITestOutputHelper testOutputHelper, Dictionary<string, object> dict, string indent = "")
    {
        foreach (var kvp in dict)
        {
            if (kvp.Value is Dictionary<string, object> nestedDict)
            {
                testOutputHelper.WriteLine($"{indent}{kvp.Key}:");
                OutputDictionary(testOutputHelper, nestedDict, indent + "  ");
            }
            else if (kvp.Value is List<Dictionary<string, object>> listDicts)
            {
                testOutputHelper.WriteLine($"{indent}{kvp.Key}: [");
                foreach (var item in listDicts)
                {
                    OutputDictionary(testOutputHelper, item, indent + "  ");
                    testOutputHelper.WriteLine($"{indent}  ---");
                }
                testOutputHelper.WriteLine($"{indent}]");
            }
            else
            {
                testOutputHelper.WriteLine($"{indent}{kvp.Key}: {kvp.Value}");
            }
        }
    }

}