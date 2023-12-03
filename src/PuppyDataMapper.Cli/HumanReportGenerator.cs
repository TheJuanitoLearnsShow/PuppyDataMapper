using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace PuppyDataMapper.Cli;

public class HumanReportGenerator()
{
    public string Transform(string? xslFilePath, 
        string mapperXmlFilePath)
    {
        XslCompiledTransform xslt = new XslCompiledTransform();
        if (xslFilePath == null)
        {
            LoadDefaultXsl(xslt);
        }
        else
        {
            xslt.Load(xslFilePath);
        }

        // Create an XmlTextWriter object with a FileStream.
        using var fs = new StringWriter();
        using var writer = XmlWriter.Create(fs);
        // Apply the transform and write the output to the console.
        xslt.Transform(mapperXmlFilePath, writer);
        return fs.ToString();
    }

    private void LoadDefaultXsl(XslCompiledTransform xslt)
    {
            // Get assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Get name of file embedded in assembly
            var resourceName = "PuppyDataMapper.Cli.Resources.data-mapper-to-html.xsl";

            // Get file as stream from assembly
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new ArgumentException($"No resource with name {resourceName}");
            }

            // Create reader for the stream
            using var reader = new StreamReader(stream);
            // Read content of the file
            xslt.Load(XmlReader.Create(reader));
    }
}