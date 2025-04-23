// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Models.Parser.Items.Directives.Common
{
    /// <inheritdoc />
    [ToolChoice(typeof(DirectiveSelector))]
    internal abstract record DirectiveConstruct : Construct
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
        ///     "DirectiveType": "Region"
        /// }
        /// </code>
        /// </example>
        public required EnDirectiveType DirectiveType { get; init; }
    }
}
