using System.Reflection;
using System.Text;

namespace PuppyMapper.Roslyn.InlineStrategy;

public class InlineMapperClassRenderer
{
    private readonly string _css;

    private string GetResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Find the full resource name (namespace + filename)
        var resourceName = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(name, StringComparison.OrdinalIgnoreCase));

        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");

        using var reader = new StreamReader(stream);
        var fileContents = reader.ReadToEnd();
        return fileContents;
    
    }
    public InlineMapperClassRenderer()
    {
        _css = GetResource("mapperRpt.css");
    }
    
    public string GenerateHtml(List<MapInstruction> maps)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<html><head>");
        sb.AppendLine("<style>");
        sb.AppendLine(_css);
        sb.AppendLine("</style>");
        sb.AppendLine("</head><body>");

        sb.AppendLine("<table>");
        sb.AppendLine("<tr><th>Output field</th><th>Mapped Value</th><th>Comments</th><th>Condition</th><th>Warnings If</th><th>Validation Errors If</th></tr>");

        foreach (var m in maps)
        {
            sb.AppendLine("<tr>");

            sb.AppendLine($"<td>{m.OutputField}</td>");
            sb.AppendLine($"<td>{m.MappedValue}</td>");
            sb.AppendLine($"<td>{m.Comment}</td>");
            sb.AppendLine($"<td>{m.Condition}</td>");

            // warnings
            sb.AppendLine("<td>");
            if (m.Warnings.Any())
            {
                sb.AppendLine("<table>");
                foreach (var w in m.Warnings)
                    sb.AppendLine($"<tr><td>{w.Condition}</td><td>{w.Message}</td></tr>");
                sb.AppendLine("</table>");
            }
            sb.AppendLine("</td>");

            // validation errors
            sb.AppendLine("<td>");
            if (m.ValidationErrors.Any())
            {
                sb.AppendLine("<table>");
                foreach (var v in m.ValidationErrors)
                    sb.AppendLine($"<tr><td>{v.Condition}</td><td>{v.Message}</td></tr>");
                sb.AppendLine("</table>");
            }
            sb.AppendLine("</td>");

            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}