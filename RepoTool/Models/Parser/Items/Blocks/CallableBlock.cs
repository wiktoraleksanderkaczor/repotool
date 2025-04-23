// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Blocks.Common;
using RepoTool.Models.Parser.Items.Common;

namespace RepoTool.Models.Parser.Items.Blocks
{
    /// <inheritdoc />
    internal sealed record CallableBlock : BlockConstruct
    {
        /// <summary>
        /// Information about the callable.
        /// </summary>
        public required CallableInfoDefinition CallableInfo { get; init; }
    }
}
