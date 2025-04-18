// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Expressions.Operators.Common;
using RepoTool.Enums.Parser.Tools.Selectors.Expressions;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions.Operators.Common
{
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