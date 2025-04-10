using RepoTool.Enums.Parser.Items.Blocks;

namespace RepoTool.Models.Parser.Items.Blocks
{
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