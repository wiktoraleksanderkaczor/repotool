using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Expressions.Operators.Common;

namespace RepoTool.Models.Parser.Items.Expressions.Operators
{
    /// <summary>
    /// Defines the possible unary operators.
    /// </summary>
    public enum EnUnaryOperator
    {
        /// <summary>
        /// Represents the logical NOT operator (!).
        /// </summary>
        LogicalNot,

        /// <summary>
        /// Represents the bitwise NOT operator (~).
        /// </summary>
        BitwiseNot,

        /// <summary>
        /// Represents the increment operator (++).
        /// </summary>
        Increment,

        /// <summary>
        /// Represents the decrement operator (--).
        /// </summary>
        Decrement,

        /// <summary>
        /// Represents the unary plus operator (+).
        /// </summary>
        UnaryPlus,

        /// <summary>
        /// Represents the unary minus operator (-).
        /// </summary>
        UnaryMinus,

        /// <summary>
        /// Address-of operator (&amp;) in C++.
        /// </summary>
        AddressOf,

        /// <summary>
        /// Dereference operator (*) in C++.
        /// </summary>
        Dereference,

        /// <summary>
        /// Python-style unpacking operator (*).
        /// </summary>
        Unpacking
    }

    /// <inheritdoc />
    public record UnaryOperatorExpression : OperatorExpression
    {
        /// <summary>
        /// The unary operator.
        /// </summary>
        /// <example>
        /// <code>
        /// !true
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Operator": "LogicalNot"
        /// }
        /// </code>
        /// </example>
        public required EnUnaryOperator Operator { get; init; }

        /// <summary>
        /// The operand of the unary operator.
        /// </summary>
        public required ExpressionConstruct? Operand { get; init; }
    }
}