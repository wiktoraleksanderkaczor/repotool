using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    public record BlockSelector : IToolSelector<EnBlockType>
    {
        /// <summary>
        /// The type of the block.
        /// </summary>
        /// <example>
        /// <code>
        /// public class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "Class"
        /// }
        /// </code>
        /// </example>
        public required EnBlockType ToolSelection { get; init; }
    }
}