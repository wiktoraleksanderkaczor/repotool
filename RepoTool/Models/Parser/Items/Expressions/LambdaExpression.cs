using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <inheritdoc />
    public record LambdaExpression : ExpressionConstruct
    {
        /// <summary>
        /// Information about the callable (lambda function).
        /// </summary>
        public required CallableInfoDefinition CallableInfo { get; init; }

        /// <summary>
        /// Lambda function body that contains the constructs to be executed.
        /// </summary>
        public required List<Construct> Constructs { get; init; }
    }
}