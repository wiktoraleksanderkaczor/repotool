// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <summary>
    /// Represents a literal value expression
    /// Only meant to be used for primitives
    /// Objects to be instanciated through callable use
    /// </summary>
    internal sealed record LiteralValueExpression : ExpressionConstruct
    {
        /// <summary>
        /// The literal value as a string.
        /// </summary>
        /// <example>
        /// <code>
        /// 123
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "LiteralValue": "123"
        /// }
        /// </code>
        /// </example>
        public required string LiteralValue { get; init; }

        /// <summary>
        /// The type information for the literal value.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }
    }
}
