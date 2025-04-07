namespace RepoTool.Enums.Inference
{
    /// <summary>
    /// Represents the type of message being sent to the inference engine
    /// </summary>
    public enum EnInferenceRole
    {
        /// <summary>
        /// Represents the system.
        /// </summary>
        System,

        /// <summary>
        /// Represents the user
        /// </summary>
        User,

        /// <summary>
        /// Represents the assistant.
        /// </summary>
        Assistant,
    }
}