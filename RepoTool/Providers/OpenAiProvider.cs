using System.ClientModel;
using Json.Schema;
using OpenAI;
using OpenAI.Chat;
using RepoTool.Enums.Inference;
using RepoTool.Extensions;

public class OpenAiProvider : IInferenceProvider
{
    private readonly ModelOptions _modelOptions;


    public OpenAiProvider(ModelOptions modelOptions)
    {
        _modelOptions = modelOptions;
    }

    public async Task<string> GetInferenceAsync(
        List<InferenceMessage> messages,
        JsonSchema jsonSchema)
    {
        ChatClient chatClient = new(
            _modelOptions.Model, 
            new ApiKeyCredential(_modelOptions.ApiKey ?? string.Empty), 
            new OpenAIClientOptions() { Endpoint = new Uri(_modelOptions.BaseUri) }
        );

        List<ChatMessage> chatMessages = messages
            .Select<InferenceMessage, ChatMessage>(m =>
            {
                return m.Role switch 
                {
                    EnInferenceRole.User => ChatMessage.CreateUserMessage(m.Content),
                    EnInferenceRole.Assistant => ChatMessage.CreateAssistantMessage(m.Content),
                    EnInferenceRole.System => ChatMessage.CreateSystemMessage(m.Content),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }).ToList();

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        ClientResult<ChatCompletion> response = await chatClient.CompleteChatAsync(
            chatMessages,
            new ChatCompletionOptions() {
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
            });
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        if (response.Value.FinishReason != ChatFinishReason.Stop)
            throw new InvalidOperationException($"Chat did not finished because of {response.Value.FinishReason}");

        return response.Value.Content.LastOrDefault()?.Text 
            ?? throw new InvalidOperationException("No content returned.");
    }
}