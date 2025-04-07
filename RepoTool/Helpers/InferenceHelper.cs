using Microsoft.EntityFrameworkCore;
using RepoTool.Enums.Inference;
using RepoTool.Extensions;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Spectre.Console;
using System.Reflection;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.Text;
using Spectre.Console.Json;
using System.Data;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatResponseFormat = Microsoft.Extensions.AI.ChatResponseFormat;
using ChatFinishReason = Microsoft.Extensions.AI.ChatFinishReason;
using System.Text.Json.Nodes;

namespace RepoTool.Helpers
{
    /// <summary>
    /// Helper class for interacting with the OpenAI API and caching inference results.
    /// </summary>
    public class InferenceHelper
    {
        private readonly RepoToolDbContext _dbContext;
        private readonly DocumentationHelper _documentationHelper;
        private readonly IOptions<InferenceOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="InferenceHelper"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="documentationHelper">The documentation helper.</param>
        /// <param name="options">The inference options.</param>
        public InferenceHelper(
            RepoToolDbContext dbContext,
            DocumentationHelper documentationHelper,
            IOptions<InferenceOptions> options)
        {
            _dbContext = dbContext;
            _documentationHelper = documentationHelper;
            _options = options;
        }

        /// <summary>
        /// Gets an inference from the inference provider, using the cache if needed.
        /// </summary>
        /// <typeparam name="TOutput">The type of the expected schema/response.</typeparam>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="inferenceRequest">The inference request.</param>
        /// <returns>The inference result, either from the cache or the API.</returns>
        /// <exception cref="ArgumentException">Thrown when TOutput is a nullable type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the API key, model name, or base URL is not set.</exception>
        public async Task<TOutput> GetInferenceAsync<TOutput, TContext>(InferenceRequest<TContext> inferenceRequest)
            where TOutput : class
            where TContext : notnull, InferenceContext
        {
            if (Nullable.GetUnderlyingType(typeof(TOutput)) != null)
            {
                throw new ArgumentException("Output type cannot be nullable.");
            }
            
            string output = await GetInferenceAsync(inferenceRequest);
            switch (typeof(TOutput)) 
            {
                case Type t when t == typeof(string):
                    return output as TOutput 
                        ?? throw new InvalidOperationException($"Failed to parse output as {typeof(TOutput).Name}.");
                case Type t when t == typeof(JsonDocument):
                    return JsonDocument.Parse(output) as TOutput 
                        ?? throw new InvalidOperationException($"Failed to parse output as {typeof(TOutput).Name}.");
                default:
                    return JsonHelper.DeserializeJsonToType<TOutput>(output);
            }
        }

        public async Task<string> GetInferenceAsync<TContext>(InferenceRequest<TContext> inferenceRequest)
            where TContext : notnull, InferenceContext
        {
            // Get the item type
            Type outputType = inferenceRequest.Context.ItemPath.GetLastObjectType();

            // Check if TSchema is nullable
            if (Nullable.GetUnderlyingType(outputType) != null)
            {
                throw new ArgumentException("Output type cannot be nullable.");
            }

            // if (outputType.IsPrimitive || outputType.IsValueType)
            // {
            //     throw new ArgumentException("Output type cannot be a primitive or value type.");
            // }
            
            // Get prompt output type
            string promptOutputType = outputType.FullName
                ?? throw new ArgumentNullException(nameof(outputType), "Output type cannot be null.");
            
            // Get or create the schema with caching if not type string
            // JsonSchema jsonSchema = await JsonHelper.GetOrCreateJsonSchemaAsync(schemaType);

            // Determine the type to use for schema generation
            Type schemaGenerationType = outputType;
            if (await JsonHelper.GetItemSchemaHandlingTypeAsync(outputType) != EnOutputHandlingType.Object)
            // if (outputType.IsPrimitive || outputType == typeof(string))
            {
                // Wrap primitive types and strings in a generic record for schema generation
                schemaGenerationType = typeof(InferenceValueWrapper<>).MakeGenericType(outputType);
            }

            // Generate the JSON schema
            JsonElement jsonSchema = AIJsonUtilities.CreateJsonSchema(
                schemaGenerationType,
                serializerOptions: JsonHelper.DefaultJsonSerializerOptions,
                inferenceOptions: new AIJsonSchemaCreateOptions());

            // Get tool item type
            Type? toolType = (inferenceRequest.Context.ItemPath.Components.LastOrDefault(c => 
                c is ItemPathToolComponent) as ItemPathToolComponent)?.ToolType;

            // Get property info
            PropertyInfo? propertyInfo = (inferenceRequest.Context.ItemPath.Components.LastOrDefault(c => 
                c is ItemPathPropertyComponent) as ItemPathPropertyComponent)?.PropertyInfo;

            // TODO: Template data too tied to parsing, needs migration to context, I think.
            TemplateData<TContext> templateData = new()
            {
                Documentation = new Documentation
                {
                    JsonSchema = jsonSchema.ToJson(),
                    ItemOutput = _documentationHelper.GetTypeDocumentation(outputType),
                    ToolOutput = toolType != null ? _documentationHelper.GetTypeDocumentation(toolType) : null,
                    PropertyInfo = propertyInfo != null ? _documentationHelper.GetPropertyDocumentation(propertyInfo) : null
                },
                Request = inferenceRequest,
                Configuration = new TemplateConfiguration()
            };
            string rawMessages = TemplateHelper.FillMessageTemplateWithContext(templateData);

            if (_options.Value.Logging.RawMessageFolder != null)
            {
                // Write rawMessages to PathConstants.RepoToolFolder within a 'logs' folder under numbered text file
                string logsFolder = _options.Value.Logging.RawMessageFolder;
                if (!Directory.Exists(logsFolder))
                {
                    Directory.CreateDirectory(logsFolder);
                }
                int fileCount = Directory.GetFiles(logsFolder, "*.txt").Length;
                string logFilePath = Path.Combine(logsFolder, $"{fileCount}.txt");
                await File.WriteAllTextAsync(logFilePath, rawMessages);
            }

            // Generate a hashed prompt key
            string promptHash = rawMessages.ToSha256Hash();

            IChatClient? chatClient = null;
            ModelOptions modelOptions = _options.Value.Configurations.GetValueOrDefault(inferenceRequest.GetInferenceReason()) 
                ?? throw new InvalidOperationException("Model options not found.");

            if (modelOptions.UseCache)
            {
                // Check cache first
                InferenceCacheEntity? cachedEntry = await _dbContext.InferenceCache
                    .FirstOrDefaultAsync(e => 
                        e.PromptHash == promptHash
                        && e.InferenceProvider == modelOptions.Provider
                        && e.OutputType == promptOutputType);

                if (cachedEntry != null)
                {
                    return cachedEntry.ResponseContent;
                }
            }

            switch (modelOptions.Provider)
            {
                case EnInferenceProvider.Ollama:
                    chatClient = new OllamaChatClient(
                        modelOptions.BaseUri,
                        modelOptions.Model);
                    break;
                case EnInferenceProvider.OpenAi:
                    
                    chatClient = new OpenAIChatClient(
                        new OpenAIClient(
                            new ApiKeyCredential(modelOptions.ApiKey ?? string.Empty), 
                            new OpenAIClientOptions { Endpoint = new Uri(modelOptions.BaseUri) }),
                        modelOptions.Model);
                    break;
                default:
                    throw new InvalidOperationException("Unknown inference provider.");
            }

            if (chatClient == null)
            {
                throw new InvalidOperationException("Chat client is null.");
            }

            List<ChatMessage> messages = ParseMessages(rawMessages);
            ChatOptions chatOptions = new()
            {
                Temperature = modelOptions.SamplingOptions.Temperature,
                TopP = modelOptions.SamplingOptions.TopP,
                FrequencyPenalty = modelOptions.SamplingOptions.FrequencyPenalty,
                PresencePenalty = modelOptions.SamplingOptions.PresencePenalty,
                Seed = modelOptions.SamplingOptions.Seed,
                // OpenAI: Invalid schema for response_format 'json_schema': schema must be a JSON Schema of 'type: "object"', got 'type: "string"'.'
                ResponseFormat = ChatResponseFormat.ForJsonSchema(jsonSchema)
            };

            // StringBuilder stringBuilder = new();
            // await foreach (ChatResponseUpdate update in chatClient.GetStreamingResponseAsync(messages, chatOptions))
            // {
            //     if (update.FinishReason != null)
            //     {
            //         ChatFinishReason finishReason = update.FinishReason.Value;
            //         // Check if the finish reason is not Stop.
            //         if (finishReason != ChatFinishReason.Stop)
            //         {
            //             // Throw an exception for any finish reason other than Stop.
            //             throw new ArgumentOutOfRangeException(nameof(update.FinishReason), $"Unexpected finish reason: {finishReason}");
            //         }
            //         // If the finish reason is Stop, break out of the loop.
            //         break;
            //     }

            //     if (update.Text != null)
            //     {
            //         stringBuilder.Append(update.Text);
            //     }
            // }

            // string? output = stringBuilder.ToString();

            ChatResponse response = await chatClient.GetResponseAsync(messages, chatOptions);
            string output = response.Text;

            // Check if output is null or empty
            if (string.IsNullOrEmpty(output))
            {
                throw new InvalidOperationException("Inference output is null or empty.");
            }

            try
            {
                JsonText jsonText = new(output);
                AnsiConsole.Write(
                    new Panel(jsonText)
                        .Header("Action")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(Color.Yellow));
            }
            catch (InvalidOperationException)
            {
                AnsiConsole.WriteLine("The output is not a valid JSON.");
                AnsiConsole.Write(
                    new Panel(output)
                        .Header("Action")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(Color.Yellow));
            }
            
            // Cache the result, if we got here, there was no cache hit
            if (modelOptions.UseCache)
            {
                try
                {
                    InferenceCacheEntity cacheEntry = new()
                    {
                        PromptHash = promptHash,
                        InferenceProvider = modelOptions.Provider,
                        OutputType = promptOutputType,
                        ResponseContent = output
                    };
                    _dbContext.InferenceCache.Add(cacheEntry);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    AnsiConsole.WriteException(e);
                    AnsiConsole.WriteLine("Error saving to database.");
                }                
            }

            // Unwrap from InferenceValueWrapper<T> to get the actual value
            if (await JsonHelper.GetItemSchemaHandlingTypeAsync(outputType) != EnOutputHandlingType.Object)
            // if (outputType.IsPrimitive || outputType == typeof(string))
            {
                // Get property name with reflection
                string propertyName = schemaGenerationType
                    .GetProperties()
                    .FirstOrDefault(p => 
                        p.PropertyType == outputType)?.Name 
                    ?? throw new InvalidOperationException($"Property of type '{outputType}' not found.");


                JsonDocument jsonDocument = JsonDocument.Parse(output);
                JsonNode jsonNode = jsonDocument.GetPropertyValue(propertyName);
                string jsonValue = jsonNode.ToJsonString();
                return jsonValue;
                // object wrappedOutput = JsonHelper.DeserializeJsonToType(output, schemaGenerationType);
                // PropertyInfo valueProperty = wrappedOutput.GetType().GetProperty("Value") 
                //     ?? throw new InvalidOperationException("Value property not found.");
                // object finalOutput = valueProperty.GetValue(wrappedOutput, null)
                //     ?? throw new InvalidOperationException("Final output is null."); 
                // return (string)finalOutput;
            }

            // Return the output
            return output;
        }

        /// <summary>
        /// Method to parse raw message strings into a list of ChatMessage objects.
        /// </summary>
        /// <param name="rawMessages">The raw message string containing message role tags.</param>
        /// <returns>A list of chat message objects.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported message role is encountered.</exception>
        private List<ChatMessage> ParseMessages(string rawMessages)
        {
            // List<InferenceMessage> messages = new();
            string messageRoles = string.Join("|", Enum.GetNames(typeof(EnInferenceRole)));
            Regex regex = new(@$"<(?<role>{messageRoles})>(?<message>[\s\S]*?)</\k<role>>", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(rawMessages);

            return matches.Select(static match =>
            {
                string role = match.Groups["role"].Value;
                string message = match.Groups["message"].Value.Trim();

                if (Enum.TryParse(role, out EnInferenceRole parsedRole))
                {
                    switch (parsedRole)
                    {
                        case EnInferenceRole.System:
                            return new ChatMessage(ChatRole.System, message);
                        case EnInferenceRole.User:
                            return new ChatMessage(ChatRole.User, message);
                        case EnInferenceRole.Assistant:
                            return new ChatMessage(ChatRole.Assistant, message); 
                        default:
                            throw new ArgumentOutOfRangeException(nameof(parsedRole), $"Unsupported message role: {parsedRole}");
                    }
                }
                
                throw new ArgumentOutOfRangeException(nameof(role), $"Unsupported message role: {role}");
            }).ToList();
        }
    }

    public record InferenceValueWrapper<T>
    {
        /// <summary>
        /// Output of a single value.
        /// </summary>
        public required T Value { get; set; }
    }
}