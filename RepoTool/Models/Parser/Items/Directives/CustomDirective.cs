// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Directives.Common;

namespace RepoTool.Models.Parser.Items.Directives
{
    /// <summary>
    /// Represents a custom preprocessor directive.
    /// </summary>
    /// <inheritdoc />
    public record CustomDirective : DirectiveConstruct
    {
        /// <summary>
        /// The raw content of the custom directive.
        /// </summary>
        /// <example>
        /// <code>
        /// #pragma ignore-warning myCustomDirective someValue
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Content": "#pragma ignore-warning myCustomDirective someValue",
        /// }
        /// </code>
        /// </example>
        public required string Content { get; init; }

        /// <summary>
        /// Description of what the custom directive does.
        /// </summary>
        /// <example>
        /// <code>
        /// #pragma ignore-warning myCustomDirective someValue
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Description": "This directive is used to ignore a specific warning.",
        /// }
        /// </code>
        /// </example>
        public required string Description { get; init; }
    }
}