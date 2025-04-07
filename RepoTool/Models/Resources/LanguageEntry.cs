public record LanguageEntry
{
    /// <summary>
    /// The language name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The language file path globbing patterns.
    /// </summary>
    public required List<string> Patterns { get; init; }
}