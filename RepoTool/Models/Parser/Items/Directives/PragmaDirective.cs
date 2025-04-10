using RepoTool.Models.Parser.Items.Directives.Common;

namespace RepoTool.Models.Parser.Items.Directives
{
    /// <summary>
    /// Represents a pragma preprocessor directive for compiler-specific instructions.
    /// </summary>
    /// <inheritdoc />
    public record PragmaDirective : DirectiveConstruct
    {
        /// <summary>
        /// The specific pragma command (e.g., "warning", "once", etc.).
        /// </summary>
        /// <example>
        /// <code>
        /// #pragma warning disable
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Command": "warning"
        /// }
        /// </code>
        /// </example>
        public required string Command { get; init; }
        
        /// <summary>
        /// The arguments provided to the pragma command.
        /// </summary>
        /// <example>
        /// <code>
        /// #pragma warning disable CS0168
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "Arguments": ["disable", "CS0168"]
        /// }
        /// </code>
        /// </example>
        public required List<string> Arguments { get; init; }
        
        /// <summary>
        /// Description of what the pragma does.
        /// </summary>
        /// <example>
        /// <code>
        /// #pragma warning disable CS0168 // Variable is declared but never used
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "Description": "Disable warning for unused variable"
        /// }
        /// </code>
        /// </example>
        public required string Description { get; init; }
    }
}