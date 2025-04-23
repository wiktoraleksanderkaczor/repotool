// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Parser.Interfaces
{
    internal interface IToolSelector<TEnum> where TEnum : struct
    {
        public TEnum ToolSelection { get; init; }
    }
}
