using Json.Schema.Generation;

/// <inheritdoc />
public record LoopingStatement : StatementConstruct
{
    /// <summary>
    /// Expression that is evaluated before the loop starts. e.g. int i = 0;
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct? Initial { get; init; }

    /// <summary>
    /// Expression that is evaluated before each iteration of the loop. e.g. i &lt; 10;
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct Condition { get; init; }
    
    /// <summary>
    /// Expression that is evaluated after each iteration of the loop. e.g. i++;
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct? Increment { get; init; }
    
    /// <summary>
    /// Ordered list of statements that are executed in the loop.
    /// </summary>
    [JsonExclude]
    public required List<Construct> Constructs { get; init; }

    /// <summary>
    /// Whether the loop is at least once. e.g. for(;;) or while(true) or do-while()
    /// </summary>
    /// <example>
    /// <code>
    /// do { ... } while(true);
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///    "IsAtLeastOnce": true
    /// }
    /// </code>
    /// </example>
    public bool IsAtLeastOnce { get; init; }
}