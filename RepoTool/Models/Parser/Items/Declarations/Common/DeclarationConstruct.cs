
using RepoTool.Attributes;
using RepoTool.Enums.Parser;


/// <inheritdoc />
[ToolChoice(typeof(DeclarationSelector))]
public abstract record DeclarationConstruct : NamedConstruct
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
    public required List<EnDeclarationModifierFlag> DeclarationModifiers { get; init; }
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
    public required List<EnAccessModifierFlag>? AccessModifiers { get; init; }
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