using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PuppyMapper.Roslyn.InlineStrategy;
public sealed class DoMapWalker : CSharpSyntaxWalker
{
    private readonly SemanticModel _model;
    public List<MapInstruction> Instructions { get; } = new();

    public DoMapWalker(SemanticModel model)
    {
        _model = model;
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var name = node.Expression.ToString();
        if (!name.EndsWith("DoMap"))
        {
            base.VisitInvocationExpression(node);
            return;
        }

        var args = node.ArgumentList.Arguments;
        var info = new MapInstruction();

        // -----------------------------
        // PARAM 1: () => _output.X = expr
        // -----------------------------
        var lambda = args[0].Expression as ParenthesizedLambdaExpressionSyntax;
        if (lambda?.Body is AssignmentExpressionSyntax assign)
        {
            if (assign.Left is MemberAccessExpressionSyntax left &&
                left.Expression.ToString() == "_output")
            {
                info.OutputField = left.Name.Identifier.Text;
            }

            info.MappedValue = assign.Right.ToString();
        }

        // -----------------------------
        // PARAM 2: comment
        // -----------------------------
        if (args.Count >= 2)
            info.Comment = args[1].Expression.ToString().Trim('"');

        // -----------------------------
        // PARAM 3: condition
        // -----------------------------
        if (args.Count >= 3)
            info.Condition = args[2].Expression.ToString();

        // -----------------------------
        // PARAM 4: warningsIf: [ (cond, msg), ... ]
        // -----------------------------
        if (args.Count >= 4)
            ExtractTupleList(args[3].Expression, info.Warnings);

        // -----------------------------
        // PARAM 5: validationErrorsIf: [ (cond, msg), ... ]
        // -----------------------------
        if (args.Count >= 5)
            ExtractTupleList(args[4].Expression, info.ValidationErrors);

        Instructions.Add(info);

        base.VisitInvocationExpression(node);
    }

    private void ExtractTupleList(ExpressionSyntax expr,
        List<(string Condition, string Message)> list)
    {
        if (expr is ImplicitArrayCreationExpressionSyntax implicitArray)
        {
            foreach (var init in implicitArray.Initializer.Expressions)
            {
                if (init is TupleExpressionSyntax tuple &&
                    tuple.Arguments.Count == 2)
                {
                    var cond = tuple.Arguments[0].Expression.ToString();
                    var msg = tuple.Arguments[1].Expression.ToString();
                    list.Add((cond, msg));
                }
            }
        }
    }
}
