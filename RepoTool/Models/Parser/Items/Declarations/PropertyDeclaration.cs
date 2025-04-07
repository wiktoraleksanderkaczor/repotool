using Json.Schema.Generation;
using RepoTool.Enums.Parser;

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
    [JsonExclude]
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