using System.Reflection;
using System.Text;

namespace PuppyMapper.Roslyn.MethodStrategy;

public class MapperClassRenderer
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
    public MapperClassRenderer()
    {
        _css = GetResource("mapperRpt.css");
    }
    
    public string GenerateHtml(List<MethodMapInfo> methods)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<html><head>");
        sb.AppendLine("<style>");
        sb.AppendLine(_css);
        sb.AppendLine("</style>");
        sb.AppendLine("</head><body>");

        
        sb.AppendLine("<table>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<th>Output field</th>");
        sb.AppendLine("<th>Mapped Value</th>");
        sb.AppendLine("<th>Comments</th>");
        sb.AppendLine("<th>Condition</th>");
        sb.AppendLine("<th>Warnings If</th>");
        sb.AppendLine("<th>Validation Errors If</th>");
        sb.AppendLine("</tr>");

        foreach (var m in methods)
        {
            foreach (var a in m.Assignments)
            {
                sb.AppendLine("<tr>");

                sb.AppendLine($"<td>{a.OutputField}</td>");
                sb.AppendLine($"<td>{a.ValueExpression}</td>");
                sb.AppendLine($"<td>{m.Summary}</td>");
                sb.AppendLine($"<td>{m.FinalCondition}</td>");

                // Warnings
                sb.AppendLine("<td>");
                if (m.Warnings.Any())
                {
                    sb.AppendLine("<table>");
                    foreach (var w in m.Warnings)
                        sb.AppendLine($"<tr><td>{w.Condition}</td><td>{w.Message}</td></tr>");
                    sb.AppendLine("</table>");
                }
                sb.AppendLine("</td>");

                // Validation errors
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
        }

        sb.AppendLine("</table>");

        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}