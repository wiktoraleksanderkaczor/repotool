// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Builders;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Builders
{
    public record IterableBuilderSelector : IToolSelector<EnIterableBuilderTool>
    {
        public required EnIterableBuilderTool ToolSelection { get; init; }
    }
}