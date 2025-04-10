using RepoTool.Enums.Parser.Items.Directives;
using RepoTool.Models.Parser.Items.Directives.Common;

namespace RepoTool.Models.Parser.Items.Directives
{
    /// <summary>
    /// Represents a preprocessor definition directive (#define, #undef).
    /// </summary>
    public record DefinitionDirective : DirectiveConstruct
    {
        /// <summary>
        /// The symbol being defined or undefined.
        /// </summary>
        /// <example>
        /// <code>
        /// #define MY_SYMBOL
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Symbol": "MY_SYMBOL"
        /// }
        /// </code>
        /// </example>
        public required string Symbol { get; init; }
        
        /// <summary>
        /// The type of definition directive.
        /// </summary>
        /// <example>
        /// <code>
        /// #define MY_SYMBOL
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "DefinitionType": "Define"
        /// }
        /// </code>
        /// </example>
        public required EnDefinitionType DefinitionType { get; init; }
        
        /// <summary>
        /// The value assigned to the symbol if applicable (for #define with value).
        /// </summary>
        /// <example>
        /// <code>
        /// #define MY_SYMBOL 10
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Value": "10"
        /// }
        /// </code>
        /// </example>
        public string? Value { get; init; }
        
        /// <summary>
        /// The purpose of this symbol definition.
        /// </summary>
        /// <example>
        /// <code>
        /// #define MY_SYMBOL // This symbol is used for debugging.
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "DefinitionPurpose": "This symbol is used for debugging."
        /// }
        /// </code>
        /// </example>
        public required string DefinitionPurpose { get; init; }
    }
}