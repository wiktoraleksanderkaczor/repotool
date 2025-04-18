// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema.Generation;
using RepoTool.Attributes.Parser;
using RepoTool.Enums.Parser;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Persistence.Entities.Common;

namespace RepoTool.Persistence.Entities
{
    public class ParsedFileEntity : BaseEntity
    {
        /// <summary>
        /// Path to the file being parsed.      
        /// </summary>
        public required string FilePath { get; init; }

        /// <summary>
        /// Content hash of the file being parsed.
        /// </summary>
        public required string FileHash { get; init; }

        /// <summary>
        /// Language ID of the file being parsed.
        /// </summary>
        public required int LanguageId { get; init; }

        /// <summary>
        /// Language entity of the file being parsed.
        /// </summary>
        public required LanguageEntity Language { get; init; }

        /// <summary>
        /// Parsed data of the file.
        /// </summary>
        public required ParsedData ParsedFile { get; init; }
    }

    public record ParsedData
    {
        /// <summary>
        /// Top-level docstring of the file being parsed, not attached to any constructs, just the file itself.
        /// </summary>
        public required string? FileDocString { get; init; }

        /// <summary>
        /// Short name of the license if any specified, not attached to any constructs, just the file itself.
        /// </summary>
        public required string? FileLicense { get; init; }

        /// <summary>
        /// Language-specific paradigm (e.g., Object-Oriented, Functional, Procedural).
        /// Apply all applicable flags for the current file.
        /// </summary>
        [UniqueItems(true)]
        [FullContentScan]
        public required HashSet<EnLanguageParadigm> LanguageParadigm { get; init; }

        /// <summary>
        /// List of constructs in the file.
        /// </summary>
        [FullContentScan]
        public required List<Construct>? Constructs { get; init; }
    }
}
