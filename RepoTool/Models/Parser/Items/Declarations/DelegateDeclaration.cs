/// <inheritdoc />
public record DelegateDeclaration : DeclarationConstruct
{
    /// <summary>
    /// Information about the callable delegate.
    /// </summary>
    public required CallableInfoDefinition CallableInfo { get; init; }
}