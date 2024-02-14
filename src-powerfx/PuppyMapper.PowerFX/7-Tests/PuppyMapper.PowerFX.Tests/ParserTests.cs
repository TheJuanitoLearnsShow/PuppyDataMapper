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
    }
}