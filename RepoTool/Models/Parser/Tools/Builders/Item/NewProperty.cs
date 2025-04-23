// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Parser.Tools.Builders.Item
{
    public record NewProperty
    {
        /// <summary>
        /// Indicates the name of the property.
        /// </summary>
        public required string PropertyName { get; init; }
    }
}
