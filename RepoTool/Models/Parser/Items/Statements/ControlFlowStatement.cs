using RepoTool.Enums.Parser.Items.Statements;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
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