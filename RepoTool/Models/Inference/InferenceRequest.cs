using RepoTool.Enums.Inference;

/// <summary>
/// Request for inference
/// </summary>
public record InferenceRequest<T> where T : notnull, InferenceContext
{
    /// <summary>
    /// Context for inference
    /// </summary>
    public required T Context { get; set; }

    /// <summary>
    /// Gets the inference reason based on the context type.
    /// </summary>
    public EnInferenceReason GetInferenceReason()
    {
        switch (Context) {
            case ChangelogContext:
                return EnInferenceReason.Changelog;
            case SummarizationContext:
                return EnInferenceReason.Summarization;
            case ParserContext:
                return EnInferenceReason.Parsing;
            default:
                throw new ArgumentOutOfRangeException(nameof(Context), Context, null);
        }
    }
}