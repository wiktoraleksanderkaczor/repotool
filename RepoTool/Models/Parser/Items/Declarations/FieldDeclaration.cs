using Json.Schema.Generation;
using RepoTool.Enums.Parser;

/// <inheritdoc />
public record FieldDeclaration : DeclarationConstruct
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
    public required List<EnAccessModifierFlag> AccessModifierFlags { get; init; }

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
    public required List<EnVariableModifierFlag> VariableModifierFlags { get; init; }

    /// <summary>
    /// The default value of the field, if any.
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct DefaultValue { get; init; }
}