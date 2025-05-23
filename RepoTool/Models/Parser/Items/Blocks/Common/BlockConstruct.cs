// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Enums.Parser.Tools.Blocks;
using RepoTool.Enums.Parser.Tools.Common;
using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Models.Parser.Items.Blocks.Common
{
    /// <inheritdoc />
    [ToolChoice(typeof(BlockSelector))]
    internal abstract record BlockConstruct : NamedConstruct
    {
        /// <summary>
        /// The type of the block.
        /// </summary>
        /// <example>
        /// <code>
        /// public class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "BlockType": "Class"
        /// }
        /// </code>
        /// </example>
        public required EnBlockType BlockType { get; init; }

        /// <summary>
        /// The list of access modifiers for this block.
        /// </summary>
        /// <example>
        /// <code>
        /// public class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "AccessModifiers": [ "Public" ]
        /// }
        /// </code>
        /// </example>
        public required List<EnAccessModifier>? AccessModifiers { get; init; }

        /// <summary>
        /// The list of inheritance modifiers for this block.
        /// </summary>
        /// <example>
        /// <code>
        /// public sealed class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "InheritanceModifiers": ["Sealed"]
        /// }
        /// </code>
        /// </example>
        public required List<EnInheritanceModifier>? InheritanceModifiers { get; init; }

        /// <summary>
        /// The list of type modifiers for this block.
        /// </summary>
        /// <example>
        /// <code>
        /// public static class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "TypeModifiers": ["Static"]
        /// }
        /// </code>
        /// </example>
        public required List<EnTypeModifier>? TypeModifiers { get; init; }

        /// <summary>
        /// The list of attributes applied to this block.
        /// </summary>
        /// <example>
        /// <code>
        /// [Serializable]
        /// public class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Attributes": [ { "Name": "Serializable", "Arguments": [] } ]
        /// }
        /// </code>
        /// </example>
        public required List<AttributeDefinition> Attributes { get; init; }

        /// <summary>
        /// List of constructs at the current level.
        /// </summary>
        public required List<Construct>? Constructs { get; init; }
    }
}
