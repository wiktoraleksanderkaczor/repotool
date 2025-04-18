// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Parser.Tools
{
    public record SummarizeAction
    {
        /// <summary>
        /// Human-readable summary of what was changed.
        /// This should be a short, one-line description of the change.
        /// It should be in the past tense and should not include any
        /// information about the change itself.
        /// e.g. "Added mortgage calculation statement"
        /// </summary>
        public required string Message { get; init; }
    }
}