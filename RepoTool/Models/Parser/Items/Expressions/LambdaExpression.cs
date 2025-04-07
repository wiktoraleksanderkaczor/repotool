using Json.Schema.Generation;


/// <inheritdoc />
public record LambdaExpression : ExpressionConstruct
{
    /// <summary>
    /// Information about the callable (lambda function).
    /// </summary>
    public required CallableInfoDefinition CallableInfo { get; init; }

    /// <summary>
    /// Lambda function body that contains the constructs to be executed.
    /// </summary>
    [JsonExclude]
    public required List<Construct> Constructs { get; init; }
}