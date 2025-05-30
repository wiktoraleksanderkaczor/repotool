// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Inference;
using RepoTool.Models.Inference.Contexts;
using RepoTool.Models.Inference.Contexts.Common;

namespace RepoTool.Models.Inference
{
    /// <summary>
    /// Request for inference
    /// </summary>
    internal sealed record InferenceRequest<T> where T : notnull, InferenceContext
    {
        /// <summary>
        /// Context for inference
        /// </summary>
        public required T Context { get; set; }
        /// <summary>
        /// Gets the inference reason based on the context type.
        /// </summary>
        public EnInferenceReason InferenceReason => Context switch
        {
            ChangelogContext => EnInferenceReason.Changelog,
            SummarizationContext => EnInferenceReason.Summarization,
            ParserContext => EnInferenceReason.Parsing,
            _ => EnInferenceReason.Unknown
        };
    }
}
