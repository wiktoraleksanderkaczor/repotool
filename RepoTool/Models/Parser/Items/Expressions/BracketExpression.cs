// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Expressions;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <inheritdoc />
    public record BracketExpression : ExpressionConstruct
    {
        /// <summary>
        /// Type of the brackets.
        /// </summary>
        /// <example>
        /// <code>
        /// [1, 2, 3]
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "BracketType": "Square"
        /// }
        /// </code>
        /// </example>
        public required EnBracketType BracketType { get; init; }

        /// <summary>
        /// Expression within the brackets.
        /// </summary>
        public required ExpressionConstruct? Expression { get; init; }
    }
}