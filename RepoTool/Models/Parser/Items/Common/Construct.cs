// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema.Generation;
using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Tools;

namespace RepoTool.Models.Parser.Items.Common
{
    /// <summary>
    /// Represents a base construct in the code.
    /// </summary>
    [ToolChoice(typeof(ConstructSelector))]
    public abstract record Construct
    {
        /// <summary>
        /// Top-level docstring for the item if applicable.
        /// </summary>
        public required string? DocString { get; init; }

        /// <summary>
        /// More specific comment for the item if applicable.
        /// </summary>
        public required string? Comment { get; init; }

        /// <summary>
        /// Line number of the item start.
        /// </summary>
        public required int LineNumber { get; init; }

        /// <summary>
        /// Purpose of the definition.
        /// </summary>
        public required string Purpose { get; init; }

        /// <summary>
        /// The type of the item.
        /// </summary>
        [JsonExclude]
        public string ItemType => GetType().Name;
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents a construct that has a name.
    /// </summary>
    public abstract record NamedConstruct : Construct
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        /// <example>
        /// <code>
        /// int myVariable;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Name": "myVariable"
        /// }
        /// </code>
        /// </example>
        public required string Name { get; init; }
    }
}
