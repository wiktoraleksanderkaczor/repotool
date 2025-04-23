// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Enums.Parser.Tools.Common;
using RepoTool.Enums.Parser.Tools.Declarations;
using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Models.Parser.Items.Declarations.Common
{
    /// <inheritdoc />
    [ToolChoice(typeof(DeclarationSelector))]
    internal abstract record DeclarationConstruct : NamedConstruct
    {
        /// <summary>
        /// The type of the declaration.
        /// </summary>
        /// <example>
        /// <code>
        /// public string MyProperty { get; set; }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "DeclarationType": "Property"
        /// }
        /// </code>
        /// </example>
        public required EnDeclarationType DeclarationType { get; init; }

        /// <summary>
        /// The list of declaration modifiers.
        /// </summary>
        /// <example>
        /// <code>
        /// public static string MyField;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "DeclarationModifiers": ["Static"]
        /// }
        /// </code>
        /// </example>
        public required List<EnDeclarationModifier> DeclarationModifiers { get; init; }
        /// <summary>
        /// The list of access modifiers.
        /// </summary>
        /// <example>
        /// <code>
        /// public string MyField;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "AccessModifiers": ["Public"]
        /// }
        /// </code>
        /// </example>
        public required List<EnAccessModifier>? AccessModifiers { get; init; }
        /// <summary>
        /// The list of attributes applied to the declaration.
        /// </summary>
        /// <example>
        /// <code>
        /// [Obsolete]
        /// public string MyField;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Attributes": [ { "Name": "Obsolete", "Arguments": [] } ]
        /// }
        /// </code>
        /// </example>
        public required List<AttributeDefinition> Attributes { get; init; }
    }
}
