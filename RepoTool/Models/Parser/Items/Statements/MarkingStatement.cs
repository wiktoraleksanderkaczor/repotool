// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Statements;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    internal sealed record MarkingStatement : StatementConstruct
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
