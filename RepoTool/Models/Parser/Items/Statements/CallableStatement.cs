using RepoTool.Models.Parser.Items.Blocks;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    public record CallableStatement : StatementConstruct
    {
        /// <summary>
        /// The callable block
        /// </summary>
        public required CallableBlock Callable { get; init; }
    }
}