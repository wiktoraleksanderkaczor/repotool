using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    public record ExpressionSelector : IToolSelector<EnExpressionType>
    {
        /// <summary>
        /// The type of the expression.
        /// </summary>
        /// <example>
        /// <code>
        /// myVariable
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "VariableUse"
        /// }
        /// </code>
        /// </example>
        public required EnExpressionType ToolSelection { get; init; }
    }
}