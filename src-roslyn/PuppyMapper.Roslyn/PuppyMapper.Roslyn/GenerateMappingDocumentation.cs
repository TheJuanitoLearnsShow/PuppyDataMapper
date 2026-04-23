using Microsoft.Build.Framework;
using PuppyMapper.Roslyn.InlineStrategy;

namespace PuppyMapper.Roslyn;

public class GenerateMappingDocumentation : Microsoft.Build.Utilities.Task
{
    [Required]
    public string SourceFile { get; set; }

    [Required]
    public string OutputFile { get; set; }

    public override bool Execute()
    {
        var code = File.ReadAllText(SourceFile);
        var parser = new InlineMappingParser();
        var methods = parser.ParseDoMapping(code);
        var renderer = new InlineMapperClassRenderer();
        var html = renderer.GenerateHtml(methods);

        File.WriteAllText(OutputFile, html);
        return true;
    }
}