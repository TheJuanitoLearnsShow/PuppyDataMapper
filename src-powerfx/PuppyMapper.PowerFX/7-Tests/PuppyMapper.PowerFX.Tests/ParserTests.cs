using Microsoft.PowerFx.Types;
using PuppyMapper.PowerFX.Service;

namespace PuppyMapper.PowerFX.Tests
{
    public class ParserTests
    {
        [Fact]
        public void TestParseSections()
        {
            using var fileContents = new StreamReader("Samples/SampleFxMapping.txt");
            var sections = MappingDocumentParser.ParseSections(fileContents).ToList();
            Assert.Equal(2, sections.Count);
            Assert.Equal(4, sections.Last().Rules.Count);
        }
        [Fact]
        public void TestExecDocument()
        {
            using var fileContents = new StreamReader("Samples/SampleFxMapping.txt");

            var doc = MappingDocumentParser.ParseMappingDocument(fileContents);
            Assert.Single(doc.MappingInputs);
            Assert.Equal("ExamStat", doc.MappingOutputType.OutputType);

            var input = FormulaValueJSON.FromJson(File.ReadAllText("Samples/SampleRecord1.json"));
            var result = MapperInterpreter.MapRecord(doc, [("input", input)]);
            Assert.Equal(4, result.Keys.Count);
        }
        [Fact]
        public void TestMapMultipleRecords()
        {
            using var fileContents = new StreamReader("Samples/SampleFxMapping.txt");

            var doc = MappingDocumentParser.ParseMappingDocument(fileContents);
            Assert.Single(doc.MappingInputs);
            Assert.Equal("ExamStat", doc.MappingOutputType.OutputType);

            var row1 = FormulaValueJSON.FromJson(File.ReadAllText("Samples/SampleRecord1.json"));
            var row2 = FormulaValueJSON.FromJson(File.ReadAllText("Samples/SampleRecord2.json"));
            var mapper = new MapperInterpreter(doc);
            var result = mapper.MapRecords([
                [("input", row1)],
                [("input", row2)]
                ]).ToList();
            Assert.Equal(2, result.Count);
        }
    }
}