// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Enums.Parser.Tools.Selectors;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Models.Parser.Items.Statements.Common
{
    /// <inheritdoc />
    [ToolChoice(typeof(StatementSelector))]
    public abstract record StatementConstruct : Construct
    {
        /// <summary>
        /// The type of the statement.
        /// </summary>
        /// <example>
        /// <code>
        /// return 0;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "StatementType": "ControlFlow"
        /// }
        /// </code>
        /// </example>
        public required EnStatementType StatementType { get; init; }

        /// <summary>
        /// The list of attributes applied to the statement.
        /// </summary>
        /// <example>
        /// <code>
        /// [Obsolete]
        /// return;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Attributes": [ { "Name": "Obsolete", "Arguments": [] } ]
        /// }
        /// </code>
        /// </example>
        public required List<AttributeDefinition> Attributes { get; init; }
    }
}
