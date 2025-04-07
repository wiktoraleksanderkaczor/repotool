using Json.Schema.Generation;

/// <inheritdoc />
public record ExceptionHandlingStatement : StatementConstruct
{
    /// <summary>
    /// The list of constructs to execute in the try block.
    /// e.g., statements, expressions, etc.
    /// </summary>
    [JsonExclude]
    public required List<Construct>? Try { get; init; }
    
    /// <summary>
    /// The list of catch blocks to handle exceptions.
    /// </summary>
    [JsonExclude]
    public required List<CatchComponent> Catches { get; init; }
    
    /// <summary>
    /// The list of constructs to execute in the finally block.
    /// e.g., statements, expressions, etc.
    /// </summary>
    [JsonExclude]
    public required List<Construct>? Finally { get; init; }
}

/// <summary>
/// Represents a catch block in a try-catch-finally statement.
/// </summary>
public record CatchComponent
{
    /// <summary>
    /// The variable that will hold the exception object.
    /// </summary>
    [JsonExclude]
    public required NewVariableExpression? Variable { get; init; }
    
    /// <summary>
    /// The list of constructs to execute if the exception is caught.
    /// e.g., statements, expressions, etc.
    /// </summary>
    [JsonExclude]
    public required List<Construct> Constructs { get; init; }
}
