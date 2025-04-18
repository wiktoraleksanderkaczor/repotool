// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Declarations;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Declarations.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Declarations
{
    /// <inheritdoc />
    public record PropertyDeclaration : DeclarationConstruct
    {
        /// <summary>
        /// Type of the property.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this property.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// public static string MyProperty { get; set; }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ModifierFlags": ["Static"]
        /// }
        /// </code>
        /// </example>
        public required List<EnPropertyModifierFlag> ModifierFlags { get; init; }

        /// <summary>
        /// Default value of the member by expression, if applicable.
        /// </summary>
        public ExpressionConstruct? DefaultValue { get; init; }

        /// <summary>
        /// Definition of the custom getter for the property if applicable.
        /// </summary>
        public required CallableInfoDefinition? Getter { get; init; }

        /// <summary>
        /// Definition of the custom setter for the property if applicable.
        /// </summary>
        public required CallableInfoDefinition? Setter { get; init; }
    }
}
