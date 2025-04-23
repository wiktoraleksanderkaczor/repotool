// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Blocks;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    internal sealed record CallableStatement : StatementConstruct
    {
        /// <summary>
        /// The callable block
        /// </summary>
        public required CallableBlock Callable { get; init; }
    }
}
