// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Repository
{
    public record SourceChange
    {
        /// <summary>
        /// The old path of the file that was changed if renamed
        /// </summary>
        public string? OldPath { get; init; }

        /// <summary>
        /// The path of the file that was changed
        /// </summary>
        public required string Path { get; init; }

        /// <summary>
        /// The patch of the file that was changed
        /// </summary>
        public required string PatchContent { get; init; }

        /// <summary>
        /// The original content of the file that was changed
        /// </summary>
        public string? OriginalContent { get; init; }

        /// <summary>
        /// Whether the file was added
        /// </summary>
        public bool IsAdded { get; init; }

        /// <summary>
        /// Whether the file was deleted
        /// </summary>
        public bool IsDeleted { get; init; }

        /// <summary>
        /// Whether the file was copied
        /// </summary>
        public bool IsCopied { get; init; }

        /// <summary>
        /// Whether the file was renamed
        /// </summary>
        public bool IsRenamed { get; init; }

        /// <summary>
        /// Whether another type of change was detected
        /// </summary>
        public bool IsOther => !IsAdded && !IsDeleted && !IsCopied && !IsRenamed;
    }
}