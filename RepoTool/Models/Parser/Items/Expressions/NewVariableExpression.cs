// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Common;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <inheritdoc />
    internal sealed record NewVariableExpression : ExpressionConstruct
    {
        /// <summary>
        /// The name of the new variable being declared.
        /// </summary>
        /// <example>
        /// <code>
        /// int myVariable = 10;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "VariableName": "myVariable"
        /// }
        /// </code>
        /// </example>
        public required string VariableName { get; init; }

        /// <summary>
        /// Type of newly made variable.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }

        /// <summary>
        /// Value of the variable by expression, if applicable i.e. if defined
        /// </summary>
        public required ExpressionConstruct? DefaultValue { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this variable.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// const int myVariable = 10;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "ModifierFlags": ["Constant"]
        /// }
        /// </code>
        /// </example>
        public required List<EnVariableModifier> ModifierFlags { get; init; }
    }
}
