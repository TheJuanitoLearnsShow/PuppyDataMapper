using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PuppyMapper.Roslyn.MethodStrategy;

public sealed class MappingWalker : CSharpSyntaxWalker
{
    private readonly SemanticModel _semanticModel;
    private readonly MethodMapInfo _info;

    public MappingWalker(SemanticModel semanticModel, MethodMapInfo info)
        : base(SyntaxWalkerDepth.StructuredTrivia)
    {
        _semanticModel = semanticModel;
        _info = info;
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        // Capture the last if condition in the method
        _info.FinalCondition = node.Condition.ToString();

        base.VisitIfStatement(node);
    }

    public override void VisitExpressionStatement(ExpressionStatementSyntax node)
    {
        if (node.Expression is AssignmentExpressionSyntax assign)
        {
            // Only capture assignments to _output.*
            if (assign.Left is MemberAccessExpressionSyntax left &&
                left.Expression.ToString() == "_output")
            {
                _info.Assignments.Add(new AssignmentInfo
                {
                    OutputField = left.Name.Identifier.Text,
                    ValueExpression = assign.Right.ToString()
                });
            }
        }

        // Capture AddWarningIf / AddValidationErrorIf
        if (node.Expression is InvocationExpressionSyntax invocation)
        {
            var name = invocation.Expression.ToString();

            if (name.EndsWith("AddWarningIf"))
            {
                var args = invocation.ArgumentList.Arguments;
                _info.Warnings.Add((args[0].ToString(), args[1].ToString()));
            }
            else if (name.EndsWith("AddValidationErrorIf"))
            {
                var args = invocation.ArgumentList.Arguments;
                _info.ValidationErrors.Add((args[0].ToString(), args[1].ToString()));
            }
        }

        base.VisitExpressionStatement(node);
    }
    

}