using RepoTool.Models.Parser.Items.Blocks.Common;


/// <inheritdoc />
public record NamespaceBlock : BlockConstruct
{
    /// <summary>
    /// Fully qualified name of the namespace
    /// e.g. MyApp.MyNamespace.InnerNamespace would be ["MyApp", "MyNamespace", "InnerNamespace"]
    /// </summary>
    /// <example>
    /// <code>
    /// namespace MyApp.MyNamespace { }
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "FullyQualifiedPath": ["MyApp", "MyNamespace"]
    /// }
    /// </code>
    /// </example>
    public required List<string>? FullyQualifiedPath { get; init; }
}
