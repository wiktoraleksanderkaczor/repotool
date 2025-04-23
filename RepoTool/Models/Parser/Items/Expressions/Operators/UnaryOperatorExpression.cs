// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Expressions.Operators;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Expressions.Operators.Common;

namespace RepoTool.Models.Parser.Items.Expressions.Operators
{
    /// <inheritdoc />
    internal sealed record UnaryOperatorExpression : OperatorExpression
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
