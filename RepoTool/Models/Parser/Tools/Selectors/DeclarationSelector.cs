// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    internal sealed record DeclarationSelector : IToolSelector<EnDeclarationType>
    {
        /// <summary>
        /// The type of the declaration.
        /// </summary>
        /// <example>
        /// <code>
        /// public string MyProperty { get; set; }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "Property"
        /// }
        /// </code>
        /// </example>
        public required EnDeclarationType ToolSelection { get; init; }
    }
}
