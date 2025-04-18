// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Directives;
using RepoTool.Models.Parser.Items.Directives.Common;

namespace RepoTool.Models.Parser.Items.Directives
{
    /// <summary>
    /// Represents a diagnostic preprocessor directive covering both warning and error directives.
    /// </summary>
    /// <inheritdoc />
    public record DiagnosticDirective : DirectiveConstruct
    {
        /// <summary>
        /// The message associated with the diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// #warning This is a warning message.
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Message": "This is a warning message."
        /// }
        /// </code>
        /// </example>
        public required string Message { get; init; }

        /// <summary>
        /// The code associated with the diagnostic, if any.
        /// </summary>
        /// <example>
        /// <code>
        /// #error CS1001
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Code": "CS1001"
        /// }
        /// </code>
        /// </example>
        public string? Code { get; init; }

        /// <summary>
        /// The severity level of the diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// #warning This is a warning message.
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "DiagnosticLevel": "Warning"
        /// }
        /// </code>
        /// </example>
        public required EnDiagnosticLevel DiagnosticLevel { get; init; }
    }
}