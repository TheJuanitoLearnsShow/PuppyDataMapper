using System.Text.RegularExpressions;
using Avalonia.Media;
using AvaloniaEdit.Highlighting;

namespace PuppyMapper.AvaloniaApp.CodeEditor;

public partial class PowerFxHighlighting
{
  public static IHighlightingDefinition Create()
    {
        var keywordsColor = new HighlightingColor
        {
            Foreground = new SimpleHighlightingBrush(Brushes.Blue.Color),
            FontWeight = FontWeight.Bold
        };

        var functionsColor = new HighlightingColor
        {
            Foreground = new SimpleHighlightingBrush(Brushes.DarkCyan.Color)
        };

        var stringColor = new HighlightingColor
        {
            Foreground = new SimpleHighlightingBrush(Brushes.Brown.Color)
        };

        var numberColor = new HighlightingColor
        {
            Foreground = new SimpleHighlightingBrush(Brushes.Magenta.Color)
        };

        var commentColor = new HighlightingColor
        {
            Foreground = new SimpleHighlightingBrush(Brushes.Green.Color),
            FontStyle = FontStyle.Italic
        };

        var operatorColor = new HighlightingColor
        {
            Foreground = new SimpleHighlightingBrush(Brushes.Chocolate.Color),
            FontWeight = FontWeight.DemiBold
        };
        
        var rules = new HighlightingRuleSet();

        rules.Rules.Add(new HighlightingRule
        {
            Regex = MyRegex(),
            Color = keywordsColor
        });

        rules.Rules.Add(new HighlightingRule
        {
            Regex = MyRegex1(),
            Color = functionsColor
        });

        rules.Rules.Add(new HighlightingRule
        {
            Regex = MyRegex2(),
            Color = stringColor
        });

        rules.Rules.Add(new HighlightingRule
        {
            Regex = MyRegex3(),
            Color = numberColor
        });

        rules.Rules.Add(new HighlightingRule
        {
            Regex = MyRegex4(),
            Color = commentColor
        });
        rules.Rules.Add(new HighlightingRule
        {
            Regex = OperatorRegx(),
            Color = operatorColor
        });
        
        return new CustomHighlightingDefinition("PowerFx", rules);
    }

    [GeneratedRegex(@"\b(?:If|Switch|With|Let|As|ThisRecord|Self|Parent|true|false)\b")]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"\b(?:Sum|Filter|LookUp|Patch|Collect|Clear|UpdateContext|Navigate|Rand|Text|Value)\b")]
    private static partial Regex MyRegex1();
    [GeneratedRegex("\".*?\"")]
    private static partial Regex MyRegex2();
    [GeneratedRegex(@"\b\d+(\.\d+)?\b")]
    private static partial Regex MyRegex3();
    [GeneratedRegex(@"//.*$")]
    private static partial Regex MyRegex4();
    [GeneratedRegex(@"(?:<=|>=|<>|==|!=|&&|\|\||:=|\+|\\|-|\*|/)")]
    private static partial Regex OperatorRegx();

}