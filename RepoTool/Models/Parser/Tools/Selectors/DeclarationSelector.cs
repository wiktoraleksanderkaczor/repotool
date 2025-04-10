using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    public record DeclarationSelector : IToolSelector<EnDeclarationType>
    {
        /// <summary>
        /// The type of the declaration.
        /// </summary>
        /// <example>
        /// <code>
        /// public string MyProperty { get; set; }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "Property"
        /// }
        /// </code>
        /// </example>
        public required EnDeclarationType ToolSelection { get; init; }
    }
}