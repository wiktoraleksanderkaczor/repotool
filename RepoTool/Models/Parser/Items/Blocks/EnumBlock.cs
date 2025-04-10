using RepoTool.Models.Parser.Items.Blocks.Common;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Blocks
{
    /// <inheritdoc />
    public record EnumBlock : BlockConstruct
    {
        /// <summary>
        /// Values of the enum.
        /// </summary>
        /// <example>
        /// <code>
        /// enum MyEnum { Value1, Value2 }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Values": [
        ///         { "Name": "Value1" },
        ///         { "Name": "Value2" }
        ///     ]
        /// }
        /// </code>
        /// </example>
        public required List<EnumValueDefinition> Values { get; init; }

        /// <summary>
        /// Underlying type of the enum (if applicable).
        /// </summary>
        /// <example>
        /// <code>
        /// enum MyEnum : byte { Value1, Value2 }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "UnderlyingType": { "Name": "byte" }
        /// }
        /// </code>
        /// </example>
        public TypeInfoDefinition? UnderlyingType { get; init; }
    }

    /// <summary>
    /// Represents a definition of an enum value.
    /// </summary>
    public record EnumValueDefinition : NamedConstruct
    {
        /// <summary>
        /// Literal value of the enum or expression for the enum value.
        /// Fill in explicit value for implicit enum values.
        /// </summary>
        public required ExpressionConstruct? ValueExpression { get; init; }
        /// <summary>
        /// Attributes for this enum value
        /// </summary>
        public required List<AttributeDefinition> Attributes { get; init; }
    }
}