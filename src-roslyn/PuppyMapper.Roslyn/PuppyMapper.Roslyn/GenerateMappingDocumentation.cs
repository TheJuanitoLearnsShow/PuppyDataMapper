using Microsoft.Build.Framework;

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
        var parser = new MapperClassParser();
        var methods = parser.ParseClass(code);
        var renderer = new MapperClassRenderer();
        var html = renderer.GenerateHtml(methods);

        File.WriteAllText(OutputFile, html);
        return true;
    }
}