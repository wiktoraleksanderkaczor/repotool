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
using System.Data;
using System.Text.Json.Nodes;
using Json.Schema;
using RepoTool.Options;
using RepoTool.Models.Inference.Contexts.Common;
using RepoTool.Models.Inference;
using RepoTool.Models.Inference.Contexts.Parser;
using RepoTool.Providers.Common;
using RepoTool.Enums.Json;

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

            // Get prompt output type
            string promptOutputType = outputType.FullName
                ?? throw new ArgumentNullException(nameof(outputType), "Output type cannot be null.");
            
            // Determine the type to use for schema generation
            EnOutputHandlingType outputHandlingType = await JsonHelper.GetItemSchemaHandlingTypeAsync(outputType);

            Type schemaGenerationType = outputHandlingType switch
            {
                EnOutputHandlingType.Object => outputType,
                _ => typeof(InferenceValueWrapper<>).MakeGenericType(outputType)
            };

            // Create JSON Schema for response object
            JsonSchema jsonSchema = await JsonHelper.GetOrCreateJsonSchemaAsync(schemaGenerationType);
            string jsonSchemaJson = jsonSchema.ToJson();

            // Get tool item type
            Type? toolType = (inferenceRequest.Context.ItemPath.Components.LastOrDefault(c => 
                c is ItemPathToolComponent) as ItemPathToolComponent)?.ToolType;

            // Get property info
            PropertyInfo? propertyInfo = (inferenceRequest.Context.ItemPath.Components.LastOrDefault(c => 
                c is ItemPathPropertyComponent) as ItemPathPropertyComponent)?.PropertyInfo;

            // Fill out data to be passed for template parsing.
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

            // Create tag wrapped messages from template data
            string rawMessages = TemplateHelper.FillMessageTemplateWithContext(templateData);

            // If raw message folder specified, save the messages.
            if (_options.Value.Logging.RawMessageFolder != null)
            {
                string logsFolder = _options.Value.Logging.RawMessageFolder;
                if (!Directory.Exists(logsFolder))
                    Directory.CreateDirectory(logsFolder);
                
                int fileCount = Directory.GetFiles(logsFolder, "*.txt").Length;
                string logFilePath = Path.Combine(logsFolder, $"{fileCount}.txt");
                await File.WriteAllTextAsync(logFilePath, rawMessages);
            }

            // Generate a hashed prompt key
            string promptHash = rawMessages.ToSha256Hash();

            // Fetch inference model options for current reason
            ModelOptions modelOptions = _options.Value.Configurations
                .GetValueOrDefault(inferenceRequest.GetInferenceReason()) 
                ?? throw new InvalidOperationException("Model options not found.");

            // If cache used, check for existing response
            if (modelOptions.UseCache)
            {
                InferenceCacheEntity? cachedEntry = await _dbContext.InferenceCache
                    .FirstOrDefaultAsync(e => 
                        e.PromptHash == promptHash
                        && e.InferenceProvider == modelOptions.Provider
                        && e.InferenceModel == modelOptions.Model
                        && e.OutputType == promptOutputType);

                if (cachedEntry != null)
                {
                    return cachedEntry.ResponseContent;
                }
            }

            // Instanciate inference provider client
            IInferenceProvider inferenceProvider = modelOptions.Provider switch
            {
                EnInferenceProvider.Ollama => new OllamaProvider(modelOptions),
                EnInferenceProvider.OpenAI => new OpenAiProvider(modelOptions),
                EnInferenceProvider.Outlines => new OutlinesProvider(modelOptions),
                _ => throw new InvalidOperationException("Unknown inference provider.")
            };

            // Parse messages into common format
            List<InferenceMessage> messages = ParseMessages(rawMessages);
            
            // Get inference provider response
            string output = await inferenceProvider.GetInferenceAsync(messages, jsonSchema);
            Environment.Exit(0);
            
            // Check if output is null or empty
            if (string.IsNullOrEmpty(output))
            {
                throw new InvalidOperationException("Inference output is null or empty.");
            }

            // Display response as JSON in console.
            output.DisplayAsJson(Color.Yellow);
            
            // If caching enabled, save response
            if (modelOptions.UseCache)
            {
                try
                {
                    InferenceCacheEntity cacheEntry = new()
                    {
                        PromptHash = promptHash,
                        InferenceProvider = modelOptions.Provider,
                        InferenceModel = modelOptions.Model,
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

            // If needed, unwrap InferenceValueWrapper<T> to get value
            if (outputHandlingType != EnOutputHandlingType.Object)
            {
                // Get property name with reflection
                string propertyName = schemaGenerationType
                    .GetProperties()
                    .FirstOrDefault(p => 
                        p.PropertyType == outputType)?.Name 
                    ?? throw new InvalidOperationException($"Property of type '{outputType}' not found.");

                JsonDocument jsonDocument = JsonDocument.Parse(output);
                JsonNode? jsonNode = jsonDocument.GetPropertyValue(propertyName);
                string jsonValue = jsonNode?.ToJsonString() ?? "null";
                return jsonValue;
            }

            // Return the object-based output
            return output;
        }

        /// <summary>
        /// Method to parse raw message strings into a list of message objects.
        /// </summary>
        /// <param name="rawMessages">The raw message string containing message role tags.</param>
        /// <returns>A list of message objects.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported message role is encountered.</exception>
        private static List<InferenceMessage> ParseMessages(string rawMessages)
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
                    return parsedRole switch
                    {
                        EnInferenceRole.System => new InferenceMessage()
                        {
                            Role = EnInferenceRole.System,
                            Content = message
                        },
                        EnInferenceRole.User => new InferenceMessage()
                        {
                            Role = EnInferenceRole.User,
                            Content = message
                        },
                        EnInferenceRole.Assistant => new InferenceMessage()
                        {
                            Role = EnInferenceRole.Assistant,
                            Content = message
                        },
                        _ => throw new ArgumentOutOfRangeException(nameof(parsedRole), $"Unsupported message role: {parsedRole}"),
                    };
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
        public required T Value { get; init; }
    }
}