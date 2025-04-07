using Json.Schema.Generation;

/// <inheritdoc />
public record CallableStatement : StatementConstruct
{
    /// <summary>
    /// The callable block
    /// </summary>
    [JsonExclude]
    public required CallableBlock Callable { get; init; }
}