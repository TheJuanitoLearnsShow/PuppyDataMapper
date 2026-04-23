using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PuppyMapper.Roslyn.InlineStrategy;

public class InlineMappingParser
{
    public List<MapInstruction> ParseDoMapping(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create("Analysis")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(tree);

        var model = compilation.GetSemanticModel(tree);
        var root = tree.GetCompilationUnitRoot();

        var doMapping = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(m => m.Identifier.Text == "DoMapping");

        if (doMapping == null)
            return [];

        var walker = new DoMapWalker(model);
        walker.Visit(doMapping.Body);

        return walker.Instructions;
    }

}