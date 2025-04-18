// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Resources
{
    public record LanguageEntry
    {
        /// <summary>
        /// The language name.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// The language file path globbing patterns.
        /// </summary>
        public required List<string> Patterns { get; init; }
    }
}