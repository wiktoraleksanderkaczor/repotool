// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <inheritdoc />
    internal sealed record ComprehensionExpression : ExpressionConstruct
    {
        /// <summary>
        /// The expression that filters the collection.
        /// </summary>
        public required ExpressionConstruct? Filter { get; init; }

        /// <summary>
        /// The expression that is evaluated for each item in the collection.
        /// </summary>
        public required ExpressionConstruct Result { get; init; }

        /// <summary>
        /// The expression that represents the collection to be iterated over.
        /// </summary>
        public required ExpressionConstruct Collection { get; init; }

        /// <summary>
        /// The type information for the result of the comprehension.
        /// </summary>
        public required TypeInfoDefinition ResultTypeInfo { get; init; }
    }
}
