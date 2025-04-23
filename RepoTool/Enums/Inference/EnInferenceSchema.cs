// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Inference
{
    /// <summary>
    /// Represents the JSON schema type to use for structured inference
    /// </summary>
    internal enum EnInferenceSchema
    {
        /// <summary>
        /// Use Ollama.
        /// </summary>
        Ollama,

        /// <summary>
        /// Use OpenAI.
        /// </summary>
        OpenAI,

        /// <summary>
        /// Use Outlines.
        /// </summary>
        Outlines,

        /// <summary>
        /// Use LM-Format-Enforcer
        /// </summary>
        LMFormatEnforcer
    }
}
