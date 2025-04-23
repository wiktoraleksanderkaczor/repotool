// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Selectors.Expressions;

namespace RepoTool.Models.Parser.Tools.Selectors.Expressions
{
    internal sealed record OperatorSelector
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
    }
}
