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
            var sections = MappingDocumentParser.ParseSections(fileContents).ToList();

            var doc = new MappingDocument("test", sections);
            var result = MapperInterpreter.MapRecord("Samples/SampleRecord.json", doc);
            Assert.Equal(4, result.Keys.Count);
        }
    }
}