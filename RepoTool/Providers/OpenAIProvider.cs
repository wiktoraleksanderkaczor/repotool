// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ClientModel;
using Json.Schema;
using OpenAI;
using OpenAI.Chat;
using RepoTool.Enums.Inference;
using RepoTool.Extensions;
using RepoTool.Models.Inference;
using RepoTool.Options;
using RepoTool.Providers.Common;

namespace RepoTool.Providers
{
    internal sealed class OpenAIProvider : IInferenceProvider
    {
        private readonly ModelOptions _modelOptions;


        public OpenAIProvider(ModelOptions modelOptions) => _modelOptions = modelOptions;

        public async Task<string> GetInferenceAsync(
            List<InferenceMessage> messages,
            JsonSchema jsonSchema)
        {
            ChatClient chatClient = new(
                _modelOptions.Model,
                new ApiKeyCredential(_modelOptions.ApiKey ?? string.Empty),
                new OpenAIClientOptions() { Endpoint = _modelOptions.BaseUrl }
            );

            List<ChatMessage> chatMessages = messages
                .Select<InferenceMessage, ChatMessage>(m =>
                {
                    return m.Role switch
                    {
                        EnInferenceRole.User => ChatMessage.CreateUserMessage(m.Content),
                        EnInferenceRole.Assistant => ChatMessage.CreateAssistantMessage(m.Content),
                        EnInferenceRole.System => ChatMessage.CreateSystemMessage(m.Content),
                        _ => throw new ArgumentOutOfRangeException(nameof(messages), $"Unsupported inference role: {m.Role}")
                    };
                }).ToList();

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            ClientResult<ChatCompletion> response = await chatClient.CompleteChatAsync(
                chatMessages,
                new ChatCompletionOptions()
                {
                    Seed = _modelOptions.SamplingOptions.Seed,
                    Temperature = _modelOptions.SamplingOptions.Temperature,
                    TopP = _modelOptions.SamplingOptions.TopP,
                    PresencePenalty = _modelOptions.SamplingOptions.PresencePenalty,
                    FrequencyPenalty = _modelOptions.SamplingOptions.FrequencyPenalty,
                    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                        "OutputFormat",
                        BinaryData.FromString(jsonSchema.ToJson()),
                        jsonSchemaIsStrict: true
                    )
                }).ConfigureAwait(false);
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            return response.Value.FinishReason != ChatFinishReason.Stop
                ? throw new InvalidOperationException($"Chat did not finished because of {response.Value.FinishReason}")
                : response.Value.Content.LastOrDefault()?.Text
                ?? throw new InvalidOperationException("No content returned.");
        }
    }
}
