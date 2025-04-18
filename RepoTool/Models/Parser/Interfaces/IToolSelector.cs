// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Parser.Interfaces
{
    public interface IToolSelector<TEnum> where TEnum : struct
    {
        public TEnum ToolSelection { get; init; }
    }
}