using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using PuppyDataMapper.Cli;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class HumanReportCommand : Command<HumanReportCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to mapper xml file.")]
        [CommandArgument(0, "<xmlMapperFilePath>")]
        [CommandOption("--xmlMapperFilePath")]
        public string XmlMapperFilePath { get; init; } = string.Empty;
        
        [Description("Path to mapper XSL file.")]
        [CommandArgument(1, "[xslFilePath]")]
        [CommandOption("--xslFilePath")]
        public string? XslFilePath { get; init; }
        
        [Description("Path to the HTML report file to create.")]
        [CommandArgument(2, "[outputFilePath]")]
        [CommandOption("--outputFilePath")]
        public string? OutputFilePath { get; init; }

    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {

        var transformer = new HumanReportGenerator();
        var rptContents = transformer.Transform(settings.XslFilePath, 
            settings.XmlMapperFilePath);
        File.WriteAllText(settings.OutputFilePath ?? Path.GetFileNameWithoutExtension(settings.XmlMapperFilePath) + ".html",
            rptContents);
        return 0;
    }
}