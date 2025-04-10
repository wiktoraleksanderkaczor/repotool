namespace RepoTool.Enums.Schemas
{
    /// <summary>
    /// Enum representing the allowed data types in OpenAI output schema.
    /// </summary>
    public enum EnOpenAiType
    {
        /// <summary>
        /// Represents a string type.
        /// </summary>
        String,

        /// <summary>
        /// Represents a numeric (floating-point) type.
        /// </summary>
        Number,

        /// <summary>
        /// Represents an integer type.
        /// </summary>
        Integer,

        /// <summary>
        /// Represents a boolean type.
        /// </summary>
        Boolean,

        /// <summary>
        /// Represents an array type.
        /// </summary>
        Array,

        /// <summary>
        /// Represents an object type.
        /// </summary>
        Object,

        /// <summary>
        /// Represents a null type.
        /// </summary>
        Null
    }
}