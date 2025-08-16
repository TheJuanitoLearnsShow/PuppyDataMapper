using Microsoft.PowerFx.Types;
using PuppyMapper.PowerFX.Service;
using PuppyMapper.PowerFX.Service.CustomLangParser;
using System.Collections.Immutable;
using System.Text.Json;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PuppyMapper.PowerFX.Tests
{
    public class ParserTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ParserTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestParseSections()
        {
            using var fileContents = new StreamReader("Samples/SampleFxMapping.txt");
            var sections = MappingDocumentParser.ParseSections(fileContents).ToList();
            Assert.Equal(2, sections.Count);
            Assert.Equal(4, sections.Last().Rules.Length);
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
            var mapper = new MapperInterpreter(doc, ImmutableDictionary<string, IMappingDocument>.Empty);
            var result = mapper.MapRecords([
                [("input", row1)],
                [("input", row2)]
                ]).ToList();
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void TestMapMultipleRecords_WithChildMappers()
        {

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            // using var fileContents = new StreamReader("Samples\\Yaml\\SampleFxMapping.yml");
            // var doc = deserializer.Deserialize<MappingDocumentDto>(fileContents);
            
            using var fileContents = new StreamReader("Samples/SampleFxMapping.txt");
            var doc = MappingDocumentParser.ParseMappingDocument(fileContents);
            
            
            var json1 = JsonSerializer.Serialize(doc, new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = null
            });
            File.WriteAllText("SampleFxMapping.json", json1);
            
            Assert.Single(doc.MappingInputs);
            Assert.Equal("ExamStat", doc.MappingOutputType.OutputType);
            
            //var yaml = serializer.Serialize(doc);
            //File.WriteAllText("sample1.yml", yaml);

            using var fileContentsChild = new StreamReader("Samples/ChildFxMapping.txt");
            var childDoc = MappingDocumentParser.ParseMappingDocument(fileContentsChild);
            
            var yaml = serializer.Serialize(childDoc);
            File.WriteAllText("childDoc.yml", yaml);

            var json2 = JsonSerializer.Serialize(childDoc,  new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = null
            });
            File.WriteAllText("ChildFxMapping.json", json2);

            var row1 = FormulaValueJSON.FromJson(File.ReadAllText("Samples/SampleRecord1.json"));
            var row2 = FormulaValueJSON.FromJson(File.ReadAllText("Samples/SampleRecord2.json"));
            var childMappers = new Dictionary<string, IMappingDocument> { { "ChildFxMapping", childDoc } } ;
            var mapper = new MapperInterpreter(doc, childMappers.ToImmutableDictionary());
            var result = mapper.MapRecords([
                [("input", row1)]
                ]).ToList();
            Assert.Single(result);
            Assert.NotNull(result[0]["MyMapping"]);
            _testOutputHelper.WriteLine(result[0]["MyMapping"].ToString());
        }
        
        [Fact]
        public async Task TestMapMultipleRecords_WithChildMappers_FromJson()
        {
            
            using var fileContents = new StreamReader("Samples/Json/SampleFxMapping.json");
            var doc = await JsonSerializer.DeserializeAsync<MappingDocumentDto>(fileContents.BaseStream);
            
            
            Assert.NotNull(doc);
            Assert.Single(doc.MappingInputs);
            Assert.Equal("ExamStat", doc.MappingOutputType.OutputType);

            using var fileContentsChild = new StreamReader("Samples/Json/ChildFxMapping.json");
            var childDoc = await JsonSerializer.DeserializeAsync<MappingDocumentDto>(fileContentsChild.BaseStream);
            Assert.NotNull(childDoc);

            var row1 = FormulaValueJSON.FromJson(await File.ReadAllTextAsync("Samples/SampleRecord1.json"));
            var row2 = FormulaValueJSON.FromJson(await File.ReadAllTextAsync("Samples/SampleRecord2.json"));
            var childMappers = new Dictionary<string, IMappingDocument> { { "ChildFxMapping", childDoc } } ;
            var mapper = new MapperInterpreter(doc, childMappers.ToImmutableDictionary());
            var result = mapper.MapRecords([
                [("input", row1)]
                ]).ToList();
            Assert.Single(result);
            Assert.NotNull(result[0]["MyMapping"]);
            _testOutputHelper.WriteLine(result[0]["MyMapping"].ToString());
            
            var resultJson = JsonSerializer.Serialize(result,  new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = null
            });
            await File.WriteAllTextAsync("result.json", resultJson);
        }
        
        //MappingDocumentEditDto
        
    }
}