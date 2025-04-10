using RepoTool.Enums.Parser.Tools.Builders;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Builders
{
    public record ObjectBuilderSelector : IToolSelector<EnObjectBuilderTool>
    {
        public required EnObjectBuilderTool ToolSelection { get; init; }
    }
}