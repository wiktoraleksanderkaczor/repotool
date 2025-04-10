using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <summary>
    /// Defines the possible types of control flow statements.
    /// </summary>
    public enum EnControlFlowStatementType
    {
        /// <summary>
        /// Represents a break statement.
        /// </summary>
        Break,

        /// <summary>
        /// Represents a continue statement.
        /// </summary>
        Continue,

        /// <summary>
        /// Represents a goto statement.
        /// </summary>
        Goto,

        /// <summary>
        /// Represents a return statement.
        /// </summary>
        Return,

        /// <summary>
        /// Represents a yield statement.
        /// </summary>
        Yield,

        /// <summary>
        /// Represents a throw statement.
        /// </summary>
        Throw
    }

    /// <inheritdoc />
    public record ControlFlowStatement : StatementConstruct
    {
        /// <summary>
        /// Type of the control flow.
        /// </summary>
        /// <example>
        /// <code>
        /// return;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ControlType": "Return"
        /// }
        /// </code>
        /// </example>
        public required EnControlFlowStatementType ControlType { get; init; }

        /// <summary>
        /// Expression for the control flow, if applicable.
        /// Return value, yield value, throw value etc.
        /// Continue and/or break would likely not have an expression.
        /// </summary>
        public required ExpressionConstruct? Expression { get; init; }
    }
}