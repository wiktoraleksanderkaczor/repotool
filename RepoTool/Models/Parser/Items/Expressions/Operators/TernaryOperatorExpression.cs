using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Expressions.Operators.Common;

namespace RepoTool.Models.Parser.Items.Expressions.Operators
{
    /// <summary>
    /// Defines the possible ternary operators.
    /// </summary>
    public enum EnTernaryOperator
    {
        /// <summary>
        /// Represents the conditional ternary operator (?:).
        /// </summary>
        ConditionalTernary,

        /// <summary>
        /// Represents a range with a step operator.
        /// </summary>
        RangeBetweenWithStep,

        /// <summary>
        /// Represents a slice with start, end, and step operator.
        /// </summary>
        SliceStartEndWithStep
    }

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