public record SummarizationContext : InferenceContext
{
    /// <summary>
    /// Content to be summarized.
    /// </summary>
    public required string Content { get; set; }
}