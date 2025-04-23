// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Json.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepoTool.Enums.Inference;
using RepoTool.Enums.Json;
using RepoTool.Extensions;
using RepoTool.Models.Inference;
using RepoTool.Models.Inference.Contexts.Common;
using RepoTool.Models.Inference.Contexts.Parser;
using RepoTool.Options;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using RepoTool.Providers;
using RepoTool.Providers.Common;
using Spectre.Console;

namespace RepoTool.Helpers
{
    /// <summary>
    /// Helper class for interacting with the OpenAI API and caching inference results.
    /// </summary>
    internal sealed class InferenceHelper
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
            if ( Nullable.GetUnderlyingType(typeof(TOutput)) != null )
            {
                throw new ArgumentException("Output type cannot be nullable.");
            }

            string output = await GetInferenceAsync(inferenceRequest).ConfigureAwait(false);
            return typeof(TOutput) switch
            {
                Type t when t == typeof(string) => output as TOutput
                                        ?? throw new InvalidOperationException($"Failed to parse output as {typeof(TOutput).Name}."),
                Type t when t == typeof(JsonDocument) => JsonDocument.Parse(output) as TOutput
                                        ?? throw new InvalidOperationException($"Failed to parse output as {typeof(TOutput).Name}."),
                _ => JsonHelper.DeserializeJsonToType<TOutput>(output),
            };
        }

        public async Task<string> GetInferenceAsync<TContext>(InferenceRequest<TContext> inferenceRequest)
            where TContext : notnull, InferenceContext
        {
            // Get the item type
            Type outputType = inferenceRequest.Context.ItemPath.LastObjectType;

            // Check if TSchema is nullable
            if ( Nullable.GetUnderlyingType(outputType) != null )
            {
                throw new ArgumentException("Output type cannot be nullable.");
            }



            // Fetch inference model options for current reason
            ModelOptions modelOptions = inferenceRequest.InferenceReason switch
            {
                EnInferenceReason.Changelog => _options.Value.Configurations.Changelog,
                EnInferenceReason.Summarization => _options.Value.Configurations.Summarization,
                EnInferenceReason.Parsing => _options.Value.Configurations.Parsing,
                EnInferenceReason.Unknown or _ => throw new InvalidOperationException("Model options not found.")
            };

            // Get prompt output type
            string promptOutputType = outputType.FullName
                ?? throw new ArgumentNullException(nameof(inferenceRequest), "Output type derived from inferenceRequest.Context.ItemPath cannot have a null FullName.");

            // Determine the type to use for schema generation
            EnSchemaOutput outputHandlingType = await JsonHelper.GetItemSchemaHandlingTypeAsync(outputType).ConfigureAwait(false);

            // Wrap the output type if not object
            Type schemaGenerationType = outputHandlingType switch
            {
                EnSchemaOutput.ObjectType => outputType,
                EnSchemaOutput.IterableType or EnSchemaOutput.ValueType => typeof(InferenceValueWrapper<>).MakeGenericType(outputType),
                _ => throw new NotImplementedException()
            };

            // Create JSON Schema for response object
            JsonSchema jsonSchema = await JsonHelper.GetOrCreateJsonSchemaAsync(
                schemaGenerationType,
                modelOptions.Provider,
                modelOptions.Schema).ConfigureAwait(false);

            // Get tool item type
            Type? toolType = ( inferenceRequest.Context.ItemPath.Components.LastOrDefault(c =>
                c is ItemPathToolComponent) as ItemPathToolComponent )?.ToolType;

            // Get property info
            PropertyInfo? propertyInfo = ( inferenceRequest.Context.ItemPath.Components.LastOrDefault(c =>
                c is ItemPathPropertyComponent) as ItemPathPropertyComponent )?.PropertyInfo;

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
            if ( _options.Value.Logging.RawMessageFolder != null )
            {
                string logsFolder = _options.Value.Logging.RawMessageFolder;
                if ( !Directory.Exists(logsFolder) )
                {
                    _ = Directory.CreateDirectory(logsFolder);
                }

                int fileCount = Directory.GetFiles(logsFolder, "*.txt").Length;
                string logFilePath = Path.Combine(logsFolder, $"{fileCount}.txt");
                await File.WriteAllTextAsync(logFilePath, rawMessages).ConfigureAwait(false);
            }

            // Generate a hashed prompt key
            string promptHash = rawMessages.ToSha256Hash();

            // If cache used, check for existing response
            if ( modelOptions.UseCache )
            {
                InferenceCacheEntity? cachedEntry = await _dbContext.InferenceCache
                    .FirstOrDefaultAsync(e =>
                        e.PromptHash == promptHash
                        && e.InferenceProvider == modelOptions.Provider
                        && e.InferenceModel == modelOptions.Model
                        && e.OutputType == promptOutputType).ConfigureAwait(false);

                if ( cachedEntry != null )
                {
                    return cachedEntry.ResponseContent;
                }
            }

            // Instanciate inference provider client
            IInferenceProvider inferenceProvider = modelOptions.Provider switch
            {
                EnInferenceProvider.Ollama => new OllamaProvider(modelOptions),
                EnInferenceProvider.OpenAI => new OpenAIProvider(modelOptions),
                EnInferenceProvider.vLLM => new VLLMProvider(modelOptions),
                EnInferenceProvider.HuggingFace => throw new NotImplementedException(),
                _ => throw new InvalidOperationException("Unknown inference provider.")
            };

            // Parse messages into common format
            List<InferenceMessage> messages = ParseMessages(rawMessages);

            // Get inference provider response
            string output = await inferenceProvider.GetInferenceAsync(messages, jsonSchema).ConfigureAwait(false);

            // Check if output is null or empty
            if ( string.IsNullOrEmpty(output) )
            {
                throw new InvalidOperationException("Inference output is null or empty.");
            }

            // Display response as JSON in console.
            output.DisplayAsJson(Color.Yellow);

            // If caching enabled, save response
            if ( modelOptions.UseCache )
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
                    _ = _dbContext.InferenceCache.Add(cacheEntry);
                    _ = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch ( DbUpdateException e )
                {
                    AnsiConsole.WriteException(e);
                    AnsiConsole.WriteLine("Error saving to database.");
                }
            }

            // If needed, unwrap InferenceValueWrapper<T> to get value
            if ( outputHandlingType != EnSchemaOutput.ObjectType )
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
            string messageRoles = string.Join("|", Enum.GetNames<EnInferenceRole>());
            Regex regex = new(@$"<(?<role>{messageRoles})>(?<message>[\s\S]*?)</\k<role>>", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(rawMessages);

            return matches.Select(static match =>
            {
                string role = match.Groups["role"].Value;
                string message = match.Groups["message"].Value.Trim();

                return Enum.TryParse(role, out EnInferenceRole parsedRole)
                    ? parsedRole switch
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
                        _ => throw new ArgumentOutOfRangeException(nameof(rawMessages), $"Unsupported message role: {parsedRole}"),
                    }
                    : throw new ArgumentOutOfRangeException(nameof(rawMessages), $"Unsupported message role: {role}");
            }).ToList();
        }
    }

    internal sealed record InferenceValueWrapper<T>
    {
        /// <summary>
        /// Output of a single value.
        /// </summary>
        public required T Value { get; init; }
    }
}
