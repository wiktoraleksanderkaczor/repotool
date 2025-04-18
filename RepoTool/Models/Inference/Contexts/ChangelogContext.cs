// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Inference.Contexts.Common;
using RepoTool.Models.Repository;

namespace RepoTool.Models.Inference.Contexts
{
    public record ChangelogContext : InferenceContext
    {
        /// <summary>
        /// Model containing original and changed data for a particular path
        /// </summary>
        public required List<SourceChange>? SourceChanges { get; set; }
    }
}