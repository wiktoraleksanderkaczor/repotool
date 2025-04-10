using RepoTool.Attributes;
using RepoTool.Enums.Parser;
using RepoTool.Models.Parser.Items.Common;

namespace RepoTool.Models.Parser.Items.Blocks.Common
{
    /// <inheritdoc />
    [ToolChoice(typeof(BlockSelector))]
    public abstract record BlockConstruct : NamedConstruct
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
        public required List<EnAccessModifierFlag>? AccessModifiers { get; init; }

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
        public required List<EnInheritanceModifierFlag>? InheritanceModifiers { get; init; }

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
        public required List<EnTypeModifierFlag>? TypeModifiers { get; init; }

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