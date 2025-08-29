using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.XmlParser;
using Xunit.Abstractions;

namespace PuppyMapper.PowerFX.Tests;

public class IDEParserTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IDEParserTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
        
    [Fact]
    public void TestParseSections_EditDTO()
    {
        var doc = new MappingDocumentEditDto
        {
            MappingRulesCode = """
                               Diff := input.Score2 - baseSalary
                               Name2 := input.Name & "-suffix" // here is amultiline comments
                                                // I can put as many comments in here, next mapping line is the one that has the ":=" symbol 
                               Total := input.Score2 + input.Score
                               RowDiff := input.Score2 - input.Score // here acomments
                               MyMapping := Map input ChildFxMapping
                               """
        };
        Assert.Equal(5, doc.MappingRules.Rules.Length);
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
            ],
            Inputs = [
                new FromCSVFileOptions() { FilePath = "Samples/CSV/SampleInput.csv" },
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