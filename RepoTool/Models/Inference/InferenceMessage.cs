using RepoTool.Enums.Inference;

namespace RepoTool.Models.Inference
{
    /// <summary>
    /// Represents a message in the inference process.
    /// </summary>
    public class InferenceMessage
    {
        /// <summary>
        /// Gets or sets the role of the message sender.
        /// </summary>
        public required EnInferenceRole Role { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public required string Content { get; set; }
    }
}