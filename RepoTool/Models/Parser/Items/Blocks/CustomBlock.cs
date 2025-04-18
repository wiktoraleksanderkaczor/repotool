// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Blocks.Common;

namespace RepoTool.Models.Parser.Items.Blocks
{
    /// <inheritdoc />
    public record CustomBlock : BlockConstruct
    {
        /// <summary>
        /// Description of the logic for/within the statement
        /// </summary>
        /// <example>
        /// <code>
        /// {
        ///    "LogicDescription": "This code is meant to calculate the sum of two numbers"
        /// }
        /// </code>
        /// </example>
        public required string LogicDescription { get; init; }

        /// <summary>
        /// Description of what the logic in a for/within is meant to do
        /// </summary>
        /// <example>
        /// <code>
        /// {
        ///   "LogicPurpose": "This code is meant to calculate the sum of two numbers"
        /// }
        /// </code>
        /// </example>
        public required string LogicPurpose { get; init; }
    }
}