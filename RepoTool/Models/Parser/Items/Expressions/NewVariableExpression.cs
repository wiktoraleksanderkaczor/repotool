using Json.Schema.Generation;
using RepoTool.Enums.Parser;

/// <inheritdoc />
public record NewVariableExpression : ExpressionConstruct
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
    [JsonExclude]
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
    public required List<EnVariableModifierFlag> ModifierFlags { get; init; }
}