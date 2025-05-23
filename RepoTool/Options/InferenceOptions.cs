// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema.Generation;
using RepoTool.Enums.Inference;
using RepoTool.Options.Common;

namespace RepoTool.Options
{
    internal sealed record InferenceOptions : IOptionModel
    {
        /// <summary>
        /// The section name for the inference options.
        /// </summary>
        public static string Section => "Inference";

        /// <summary>
        /// Logging configuration for the inference options.
        /// </summary>
        public LoggingOptions Logging { get; set; } = new();

        /// <summary>
        /// The type of inference to use.
        /// </summary>
        public Configurations Configurations { get; set; } = new();
    }

    internal sealed record Configurations
    {
        [Required]
        public ModelOptions Summarization { get; set; } = new();

        [Required]
        public ModelOptions Parsing { get; set; } = new();

        [Required]
        public ModelOptions Changelog { get; set; } = new();
    }

    internal sealed record ModelOptions
    {
        /// <summary>
        /// The type of inference provider to use.
        /// </summary>
        public EnInferenceProvider Provider { get; set; }

        /// <summary>
        /// The type of JSON Schema to use for inference
        /// </summary>
        public EnInferenceSchema Schema { get; set; }

        /// <summary>
        /// The model to use for inference.
        /// </summary>
        public string Model { get; set; } = "gpt-4o-mini";

        /// <summary>
        /// Whether to use and store cached results.
        /// </summary>
        public bool UseCache { get; set; } = true;

        /// <summary>
        /// The options for inference.
        /// </summary>
        public SamplingOptions SamplingOptions { get; set; } = new();

        /// <summary>
        /// The URL of the API server.
        /// </summary>
        public Uri BaseUrl { get; set; } = new("https://api.openai.com/v1/");

        /// <summary>
        /// API key for the server.
        /// </summary>
        public string? ApiKey { get; set; }
    }

    /// <summary>
    /// Represents the sampling options for text generation.
    /// </summary>
    internal sealed record SamplingOptions
    {
        /// <summary>
        /// Number of tokens used for generating next token
        /// </summary>
        public int NumContext { get; set; } = 4096;

        /// <summary>
        /// Seed for random number generation.
        /// </summary>
        public int Seed { get; set; } = 666;

        /// <summary>
        /// The temperature for sampling.
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// The top-p sampling parameter.
        /// </summary>
        public float TopP { get; set; } = 0.5f;

        /// <summary>
        /// The min-p sampling parameter.
        /// </summary>
        public float? MinP { get; set; }

        /// <summary>
        /// The top-k sampling parameter.
        /// </summary>
        public int? TopK { get; set; }

        /// <summary>
        /// The frequency penalty for generated text.
        /// </summary>
        public float FrequencyPenalty { get; set; }

        /// <summary>
        /// The presence penalty for generated text.
        /// </summary>
        public float PresencePenalty { get; set; }
    }

    internal sealed record LoggingOptions
    {
        /// <summary>
        /// The path to the log file.
        /// </summary>
        public string? RawMessageFolder { get; set; }
    }
}
