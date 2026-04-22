using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PuppyMapper.Roslyn;

public class MapperClassParser
{
    public List<MethodMapInfo> ParseClass(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create("Analysis")
            .AddReferences(
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);

        var model = compilation.GetSemanticModel(tree);

        var root = tree.GetCompilationUnitRoot();

        return root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .Select(m => ExtractMethodInfo(m, model))
            .ToList();
    }

    public MethodMapInfo ExtractMethodInfo(MethodDeclarationSyntax method, SemanticModel model)
    {
        var info = new MethodMapInfo
        {
            MethodName = method.Identifier.Text
        };

        // XML doc comments
        var trivia = method.GetLeadingTrivia()
            .Select(i => i.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>()
            .FirstOrDefault();

        if (trivia != null)
        {
            var summary = trivia.Content
                .OfType<XmlElementSyntax>()
                .FirstOrDefault(e => e.StartTag.Name.LocalName.Text == "summary");

            info.Summary = summary?.Content.ToString().Trim();

            foreach (var param in trivia.Content.OfType<XmlElementSyntax>()
                         .Where(e => e.StartTag.Name.LocalName.Text == "param"))
            {
                var name = param.StartTag.Attributes
                    .OfType<XmlNameAttributeSyntax>()
                    .First().Identifier.Identifier.Text;

                info.ParameterComments.Add((name, param.Content.ToString().Trim()));
            }
        }

        // Walk the body
        var walker = new MappingWalker(model, info);
        walker.Visit(method.Body);

        return info;
    }
}