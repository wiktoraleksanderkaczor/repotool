using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <summary>
    /// Defines the possible types of marking statements.
    /// </summary>
    public enum EnMarkingStatementType
    {
        /// <summary>
        /// Represents a label.
        /// </summary>
        Label,

        // TODO: Worry about SQL-like query languages later...
        // Transaction,
        // Rollback,
        // Commit,
        // Savepoint,
    }

    /// <inheritdoc />
    public record MarkingStatement : StatementConstruct
    {
        /// <summary>
        /// The type of marking statement.
        /// </summary>
        /// <example>
        /// <code>
        /// myLabel:
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "MarkingType": "Label"
        /// }
        /// </code>
        /// </example>
        public required EnMarkingStatementType MarkingType { get; init; }

        /// <summary>
        /// Expression for the marking, if applicable.
        /// Labels would likely have an expression for the label name.
        /// Transactions would likely have an expression for the transaction name.
        /// </summary>
        public required ExpressionConstruct? Expression { get; init; }
    }
}