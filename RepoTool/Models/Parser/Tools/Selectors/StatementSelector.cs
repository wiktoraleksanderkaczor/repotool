using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    public record StatementSelector : IToolSelector<EnStatementType>
    {
        /// <summary>
        /// The type of the statement.
        /// </summary>
        /// <example>
        /// <code>
        /// return 0;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "ControlFlow"
        /// }
        /// </code>
        /// </example>
        public required EnStatementType ToolSelection { get; init; }
    }
}