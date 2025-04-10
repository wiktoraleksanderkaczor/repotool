using RepoTool.Enums.Parser.Items.Expressions.Operators;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Expressions.Operators.Common;

namespace RepoTool.Models.Parser.Items.Expressions.Operators
{
    /// <inheritdoc />
    public record BinaryOperatorExpression : OperatorExpression
    {
        /// <summary>
        /// The binary operator.
        /// </summary>
        /// <example>
        /// <code>
        /// a + b
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Operator": "Add"
        /// }
        /// </code>
        /// </example>
        public required EnBinaryOperator Operator { get; init; }

        /// <summary>
        /// The left operand of the binary operator.
        /// </summary>
        public required ExpressionConstruct? Left { get; init; }
        
        /// <summary>
        /// The right operand of the binary operator.
        /// </summary>
        public required ExpressionConstruct? Right { get; init; }
    }
}