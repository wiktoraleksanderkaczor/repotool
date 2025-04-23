// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Inference.Contexts.Common;
using RepoTool.Models.Inference.Contexts.Parser;

namespace RepoTool.Models.Inference.Contexts
{
    internal sealed record ParserContext : InferenceContext
    {
        /// <summary>
        /// Path to the file being parsed.      
        /// </summary>
        public required string FilePath { get; init; }

        /// <summary>
        /// Content of the code window to be processed.
        /// </summary>
        public required CodeWindow CodeWindow { get; set; }

        /// <summary>
        /// Log of actions taken during parsing.
        /// </summary>
        public required ActionWindow ActionWindow { get; set; }
    }
}
