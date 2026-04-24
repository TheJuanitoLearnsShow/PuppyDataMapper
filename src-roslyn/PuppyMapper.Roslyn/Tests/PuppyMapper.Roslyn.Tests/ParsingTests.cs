using PuppyMapper.Roslyn.InlineStrategy;

namespace PuppyMapper.Roslyn.Tests;

public class ParsingTests
{
    [Fact]
    public void InlineMappingParsing()
    {
        var sourceFile = Path.Combine(Directory.GetCurrentDirectory(), 
            "TestData", "StudentMapper.cs");
        var code = File.ReadAllText(sourceFile);
        var parser = new InlineMappingParser();
        var methods = parser.ParseDoMapping(code);
        var renderer = new InlineMapperClassRenderer();
        var html = renderer.GenerateHtml(methods);

        File.WriteAllText("Test.html", html);
    }
}