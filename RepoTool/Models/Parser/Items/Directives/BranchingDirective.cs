// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Directives.Common;

namespace RepoTool.Models.Parser.Items.Directives
{
    /// <summary>
    /// Represents a preprocessor branching directive (#if, #elif, #else, #endif).
    /// </summary>
    public record BranchingDirective : DirectiveConstruct
    {
        /// <summary>
        /// The condition expression for the branch.
        /// </summary>
        /// <example>
        /// <code>
        /// {
        ///    "Condition": "SOME_CONDITION"
        /// }
        /// </code>
        /// </example>
        public required string Condition { get; init; }

        /// <summary>
        /// The default condition if nothing else matches for the branch
        /// </summary>
        public required List<Construct>? Default { get; init; }

        /// <summary>
        /// Ordered list of branches because order matters.
        /// </summary>
        public required List<DirectiveBranchComponent> Branches { get; init; }
    }

    public record DirectiveBranchComponent
    {
        /// <summary>
        /// The condition expression for the branch.
        /// </summary>
        /// <example>
        /// <code>
        /// {
        ///   "Condition": "SOME_OTHER_CONDITION"
        /// }
        /// </code>
        /// </example>
        public required string Condition { get; init; }

        /// <summary>
        /// Contents of the branch.
        /// </summary>
        public required List<Construct> Contents { get; init; }
    }
}
