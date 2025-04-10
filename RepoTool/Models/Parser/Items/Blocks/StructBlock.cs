namespace RepoTool.Models.Parser.Items.Blocks
{
    /// <summary>
    /// Defines the possible modifier flags for a struct.
    /// </summary>
    public enum EnStructModifierFlag
    {
        /// <summary>
        /// Indicates that the struct represents a union.
        /// </summary>
        Union,
    }

    /// <inheritdoc />
    public record StructBlock : ClassBlock
    {
        /// <summary>
        /// List of all applicable modifier flags for this struct.
        /// </summary>
        /// <example>
        /// <code>
        /// public union MyUnion { ... }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "StructModifierFlags": ["Union"]
        /// }
        /// </code>
        /// </example>
        public required List<EnStructModifierFlag> StructModifierFlags { get; init; }
    }
}