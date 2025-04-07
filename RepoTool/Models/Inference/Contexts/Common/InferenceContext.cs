public record InferenceContext
{
    /// <summary>
    /// Item path of the section currently being parsed.
    /// Null if not applicable i.e. top-level
    /// </summary>
    public required ItemPath ItemPath { get; set; }
}

