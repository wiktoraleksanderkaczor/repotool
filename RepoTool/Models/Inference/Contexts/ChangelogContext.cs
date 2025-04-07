public record ChangelogContext : InferenceContext
{
    /// <summary>
    /// Model containing original and changed data for a particular path
    /// </summary>
    public required List<SourceChange>? SourceChanges { get; set; } 
}