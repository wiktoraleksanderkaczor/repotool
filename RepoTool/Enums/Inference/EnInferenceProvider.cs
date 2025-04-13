namespace RepoTool.Enums.Inference
{
    /// <summary>
    /// Represents the provider to use for inference.
    /// </summary>
    public enum EnInferenceProvider
    {
        /// <summary>
        /// Use Ollama for inference.
        /// </summary>
        Ollama,

        /// <summary>
        /// Use OpenAI for inference.
        /// </summary>
        OpenAI,

        /// <summary>
        /// Use vLLM (Outlines) for inference.
        /// </summary>
        Outlines
    }
}