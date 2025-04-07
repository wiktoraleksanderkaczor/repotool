using Json.Schema.Generation;

/// <inheritdoc />
public record ComprehensionExpression : ExpressionConstruct
{
    /// <summary>
    /// The expression that filters the collection.
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct? Filter { get; init; }
    
    /// <summary>
    /// The expression that is evaluated for each item in the collection.
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct Result { get; init; }
    
    /// <summary>
    /// The expression that represents the collection to be iterated over.
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct Collection { get; init; }

    /// <summary>
    /// The type information for the result of the comprehension.
    /// </summary>
    public required TypeInfoDefinition ResultTypeInfo { get; init; }
}