// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Interfaces;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    public record DirectiveSelector : IToolSelector<EnDirectiveType>
    {
        /// <summary>
        /// The type of the preprocessor directive.
        /// </summary>
        /// <example>
        /// <code>
        /// #region MyRegion
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "Region"
        /// }
        /// </code>
        /// </example>
        public required EnDirectiveType ToolSelection { get; init; }
    }
}