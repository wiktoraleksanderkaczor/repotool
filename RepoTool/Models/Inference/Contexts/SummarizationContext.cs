using RepoTool.Models.Inference.Contexts.Common;

namespace RepoTool.Models.Inference.Contexts
{
    public record SummarizationContext : InferenceContext
    {
        /// <summary>
        /// Content to be summarized.
        /// </summary>
        public required string Content { get; set; }
    }
}