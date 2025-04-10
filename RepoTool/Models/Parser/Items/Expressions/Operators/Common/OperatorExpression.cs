using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Tools.Selectors.Expressions;

namespace RepoTool.Models.Parser.Items.Expressions.Operators.Common
{
    /// <summary>
    /// Defines the possible associativities of operators.
    /// </summary>
    public enum EnOperatorAssociativity
    {
        /// <summary>
        /// Left-to-right associativity.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Right-to-left associativity.
        /// </summary>
        RightToLeft
    }

    /// <inheritdoc />
    public abstract record OperatorExpression : ExpressionConstruct
    {
        /// <summary>
        /// The type of the operator.
        /// </summary>
        /// <example>
        /// <code>
        /// a + b
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "OperatorType": "Binary"
        /// }
        /// </code>
        /// </example>
        public required EnOperatorType OperatorType { get; init; }

        /// <summary>
        /// The associativity of the operator.
        /// </summary>
        /// <example>
        /// <code>
        /// a + b + c
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Associativity": "LeftToRight"
        /// }
        /// </code>
        /// </example>
        public required EnOperatorAssociativity? Associativity { get; init; }
    }
}