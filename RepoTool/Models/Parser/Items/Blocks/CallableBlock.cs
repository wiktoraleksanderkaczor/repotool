using RepoTool.Models.Parser.Items.Blocks.Common;
using RepoTool.Models.Parser.Items.Common;

namespace RepoTool.Models.Parser.Items.Blocks
{
    /// <inheritdoc />
    public record CallableBlock : BlockConstruct
    {
        /// <summary>
        /// Information about the callable.
        /// </summary>
        public required CallableInfoDefinition CallableInfo { get; init; }
    }
}