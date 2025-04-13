using RepoTool.Enums.Inference;
using RepoTool.Options.Common;

namespace RepoTool.Options
{
    public record InferenceOptions : IOptionModel
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
        public Dictionary<EnInferenceReason, ModelOptions> Configurations { get; set; } = new()
    {
        { EnInferenceReason.Summarization, new ModelOptions() },
        { EnInferenceReason.Parsing, new ModelOptions() },
        { EnInferenceReason.Changelog, new ModelOptions() },
    };
    }

    public record ModelOptions
    {
        /// <summary>
        /// The type of inference provider to use.
        /// </summary>
        public EnInferenceProvider Provider { get; set; } = EnInferenceProvider.OpenAI;

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
        public string BaseUri { get; set; } = "https://api.openai.com/v1/";

        /// <summary>
        /// API key for the server.
        /// </summary>
        public string? ApiKey { get; set; }
    }

    /// <summary>
    /// Represents the sampling options for text generation.
    /// </summary>
    public record SamplingOptions
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
        public float Temperature { get; set; } = 0;

        /// <summary>
        /// The top-p sampling parameter.
        /// </summary>
        public float TopP { get; set; } = 0.5f;

        /// <summary>
        /// The frequency penalty for generated text.
        /// </summary>
        public float FrequencyPenalty { get; set; } = 0.0f;

        /// <summary>
        /// The presence penalty for generated text.
        /// </summary>
        public float PresencePenalty { get; set; } = 0.0f;
    }

    public record LoggingOptions
    {
        /// <summary>
        /// The path to the log file.
        /// </summary>
        public string? RawMessageFolder { get; set; } = null;
    }
}