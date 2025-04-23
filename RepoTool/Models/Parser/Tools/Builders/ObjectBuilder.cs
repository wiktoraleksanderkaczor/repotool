// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Builders;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Builders
{
    internal sealed record ItemBuilderSelector : IToolSelector<EnItemBuilderTool>
    {
        public required EnItemBuilderTool ToolSelection { get; init; }
    }
}
