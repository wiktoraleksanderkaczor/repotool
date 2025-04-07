using RepoTool.Attributes;


/// <inheritdoc />
[ToolChoice(typeof(DirectiveSelector))]
public abstract record DirectiveConstruct : Construct
{
    /// <summary>
    /// The type of the preprocessor directive.
    /// </summary>
    /// <example>
    /// <code>
    /// #region MyRegion
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "DirectiveType": "Region"
    /// }
    /// </code>
    /// </example>
    public required EnDirectiveType DirectiveType { get; init; }
}