using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    public record CustomStatement : StatementConstruct
    {
        /// <summary>
        /// Description of the logic for/within the statement
        /// </summary>
        public required string LogicDescription { get; init; }

        /// <summary>
        /// Description of what the logic in a for/within is meant to do
        /// </summary>
        public required string LogicPurpose { get; init; }
    }
}