namespace RepoTool.Enums.Inference
{
    /// <summary>
    /// Represents the reason that inference was called
    /// </summary>
    public enum EnInferenceReason
    {
        /// <summary>
        /// Determine changelog entries
        /// </summary>
        Changelog,

        /// <summary>
        /// Summarize content of a collection, file or code
        /// </summary>
        Summarization,

        /// <summary>
        /// Parse a file into a language-agnostic representation
        /// </summary>
        Parsing
    }
}