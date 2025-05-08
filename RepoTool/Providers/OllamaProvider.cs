// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Text;
using System.Text.Json.Nodes;
using Json.Schema;
using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;
using RepoTool.Enums.Inference;
using RepoTool.Extensions;
using RepoTool.Models.Inference;
using RepoTool.Options;
using RepoTool.Providers.Common;

namespace RepoTool.Providers
{
    internal sealed class OllamaProvider : IInferenceProvider
    {
        private readonly ModelOptions _modelOptions;
        public OllamaProvider(ModelOptions modelOptions) => _modelOptions = modelOptions;

        public async Task<string> GetInferenceAsync(
            List<InferenceMessage> messages,
            JsonSchema jsonSchema)
        {
            using OllamaApiClient ollama = new(_modelOptions.BaseUrl);
            ChatRequest chatRequest = new()
            {
                Model = _modelOptions.Model,
                Messages = messages.Select(m => new Message
                {
                    Role = m.Role switch
                    {
                        EnInferenceRole.User => ChatRole.User,
                        EnInferenceRole.Assistant => ChatRole.Assistant,
                        EnInferenceRole.System => ChatRole.System,
                        _ => ChatRole.System
                    },
                    Content = m.Content
                }),
                Options = new RequestOptions
                {
                    Seed = _modelOptions.SamplingOptions.Seed,
                    NumCtx = _modelOptions.SamplingOptions.NumContext,
                    NumGpu = null, // Auto-detect
                    NumKeep = -1, // TODO: Wut dis?
                    NumThread = Environment.ProcessorCount,
                    NumPredict = -1, // -1 means no limit, -2 means fill context
                    TopK = _modelOptions.SamplingOptions.TopK,
                    Temperature = _modelOptions.SamplingOptions.Temperature,
                    TopP = _modelOptions.SamplingOptions.TopP,
                    MinP = _modelOptions.SamplingOptions.MinP,
                    PresencePenalty = _modelOptions.SamplingOptions.PresencePenalty,
                    FrequencyPenalty = _modelOptions.SamplingOptions.FrequencyPenalty
                },
                Format = JsonNode.Parse(jsonSchema.ToJson())
            };

            try
            {
                StringBuilder sb = new();
                await foreach ( ChatResponseStream? response in ollama.ChatAsync(chatRequest).ConfigureAwait(false) )
                {
                    if ( response != null )
                    {
                        if ( response.Done )
                        {
                            break;
                        }

                        sb = sb.Append(response.Message?.Content ?? string.Empty);
                    }
                }

                // Return the accumulated response content
                return sb.ToString();
            }
            catch ( Exception ex )
            {
                throw new InvalidOperationException("An error occurred during inference.", ex);
            }
        }
    }
}
