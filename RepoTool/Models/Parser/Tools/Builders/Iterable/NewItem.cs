// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Builders.Iterable;

namespace RepoTool.Models.Parser.Tools.Builders.Iterable
{
    internal sealed record NewItem
    {
        /// <summary>
        /// Indicates whether a new item has been created.
        /// </summary>
        public required EnIterableInsertAt InsertAt { get; init; }
    }
}
