using RepoTool.Enums.Parser.Items.Expressions.Operators;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Expressions.Operators.Common;

namespace RepoTool.Models.Parser.Items.Expressions.Operators
{
    /// <inheritdoc />
    public record TernaryOperatorExpression : OperatorExpression
    {
        /// <summary>
        /// The ternary operator.
        /// </summary>
        /// <example>
        /// <code>
        /// condition ? consequent : alternative
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Operator": "ConditionalTernary"
        /// }
        /// </code>
        /// </example>
        public required EnTernaryOperator Operator { get; init; }

        /// <summary>
        /// The condition or step of the ternary operator.
        /// </summary>
        public required ExpressionConstruct? Condition { get; init; }
        
        /// <summary>
        /// The left operand of the ternary operator.
        /// </summary>
        public required ExpressionConstruct? Left { get; init; }
        
        /// <summary>
        /// The right operand of the ternary operator.
        /// </summary>
        public required ExpressionConstruct? Right { get; init; }
    }
}