/// <inheritdoc />
public record CallableBlock : BlockConstruct
{
    /// <summary>
    /// Information about the callable.
    /// </summary>
    public required CallableInfoDefinition CallableInfo { get; init; }
}