// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Inference.Contexts.Common;

namespace RepoTool.Models.Inference.Contexts
{
    public record SummarizationContext : InferenceContext
    {
        /// <summary>
        /// Content to be summarized.
        /// </summary>
        public required string Content { get; set; }
    }
}