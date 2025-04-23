// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Inference.Contexts.Parser;

namespace RepoTool.Models.Inference.Contexts.Common
{
    internal record InferenceContext
    {
        /// <summary>
        /// Item path of the section currently being parsed.
        /// Null if not applicable i.e. top-level
        /// </summary>
        public required ItemPath ItemPath { get; set; }
    }
}
