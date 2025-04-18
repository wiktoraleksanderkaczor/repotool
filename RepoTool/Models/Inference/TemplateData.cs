// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Documentation;
using RepoTool.Models.Inference.Contexts.Common;

namespace RepoTool.Models.Inference
{
    public record TemplateData<T> where T : notnull, InferenceContext
    {
        /// <summary>
        /// The inference request plus context.
        /// </summary>
        public required InferenceRequest<T> Request { get; init; }

        /// <summary>
        /// Output and related type documentation.
        /// </summary>
        public required Documentation Documentation { get; init; }

        /// <summary>
        /// Template configuration.
        /// </summary>
        public required TemplateConfiguration Configuration { get; init; }
    }

    /// <summary>
    /// Configuration for the template.
    /// e.g. use full file context or window context. etc.
    /// </summary>
    public record TemplateConfiguration;

    /// <summary>
    /// Documentation for the inference request.
    /// </summary>
    public record Documentation
    {
        /// <summary>
        /// The JSON schema to be used for inference.
        /// </summary>
        public required string JsonSchema { get; init; }

        /// <summary>
        /// Output type documentation.
        /// </summary>
        public required TypeDocumentation ItemOutput { get; init; }

        /// <summary>
        /// Tool output type documentation. Null if not using a tool.
        /// </summary>
        public required TypeDocumentation? ToolOutput { get; init; }

        /// <summary>
        /// Property currently being processed.
        /// If builder, then this is the property being built.
        /// Will be null for one-off top-level values like 'string' or 'int'.
        /// </summary>
        public required PropertyDocumentation? PropertyInfo { get; init; }
    }
}