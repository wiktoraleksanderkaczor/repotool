// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Common;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Declarations.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Declarations
{
    /// <inheritdoc />
    internal sealed record FieldDeclaration : DeclarationConstruct
    {
        /// <summary>
        /// The type of the field.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }

        /// <summary>
        /// The access modifiers for the field.
        /// </summary>
        /// <example>
        /// <code>
        /// public string MyField;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "AccessModifierFlags": ["Public"]
        /// }
        /// </code>
        /// </example>
        public required List<EnAccessModifier> AccessModifierFlags { get; init; }

        /// <summary>
        /// The variable modifiers for the field.
        /// </summary>
        /// <example>
        /// <code>
        /// static readonly string MyField;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "VariableModifierFlags": ["Static", "ReadOnly"]
        /// }
        /// </code>
        /// </example>
        public required List<EnVariableModifier> VariableModifierFlags { get; init; }

        /// <summary>
        /// The default value of the field, if any.
        /// </summary>
        public required ExpressionConstruct DefaultValue { get; init; }
    }
}
