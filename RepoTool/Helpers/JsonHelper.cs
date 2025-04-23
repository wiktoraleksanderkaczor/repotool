// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Force.DeepCloner;
using Json.Schema;
using Json.Schema.Generation;
using RepoTool.Constants;
using RepoTool.Enums.Inference;
using RepoTool.Enums.Json;
using RepoTool.Extensions;
using RepoTool.Schemas;
using RepoTool.Schemas.Generators;
using RepoTool.Schemas.Refiners;
using PropertyNameResolvers = Json.Schema.Generation.PropertyNameResolvers;

namespace RepoTool.Helpers
{
    /// <summary>
    /// Provides helper methods for JSON serialization, deserialization, and schema generation.
    /// </summary>
    public static class JsonHelper
    {
        public static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = new()
        {
            // Convert enum values to strings
            Converters = { new JsonStringEnumConverter(allowIntegerValues: false) },
            // Do not allow missing members
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            // Nulls included by default when deserializing
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
            PropertyNameCaseInsensitive = true
        };

        public static SchemaGeneratorConfiguration DefaultSchemaGeneratorConfiguration { get; } = new()
        {
            // Ensure consistent order
            PropertyOrder = PropertyOrder.AsDeclared,
            PropertyNameResolver = PropertyNameResolvers.PascalCase,
            // Add custom schema generators if needed
            Generators = { new CharSchemaGenerator() },
            Refiners = { new AdditionalPropertiesRefiner() }
        };

        /// <summary>
        /// Serializes an object to a JSON string using System.Text.Json.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string SerializeToJson(object obj) =>
            // Use System.Text.Json for serializing the object
            JsonSerializer.Serialize(obj, DefaultJsonSerializerOptions);

        /// <summary>
        /// Gets the JsonSchema object for the specified type, creating and caching it if necessary.
        /// </summary>
        /// <param name="type">The type for which to get the schema object.</param>
        /// <param name="provider">Optional inference provider for schema generation.</param>
        /// <param name="schema">Optional schema type for generation.</param>
        /// <returns>A task representing the asynchronous operation, containing the <see cref="JsonSchema"/> object.</returns>
        public static async Task<JsonSchema> GetOrCreateJsonSchemaAsync(Type type, EnInferenceProvider? provider = null, EnInferenceSchema? schema = null)
        {
            // Create schemas directory if it doesn't exist
            if ( !Directory.Exists(PathConstants.UserRepoToolSchemaFolder) )
            {
                Directory.CreateDirectory(PathConstants.UserRepoToolSchemaFolder);
            }

            // Compute hash of the type to use as cache key
            string typeHash = ComputeTypeHash(type);
            string schemaPath = Path.Combine(PathConstants.UserRepoToolSchemaFolder, $"schema-{typeHash}.json");

#if !DEBUG
            // Check if schema file exists
            if (File.Exists(schemaPath))
            {
                try
                {
                    Console.WriteLine($"Using cached schema for {type.Name} from {schemaPath}"); // Use logger in production
                    string schemaJson = await File.ReadAllTextAsync(schemaPath);
                    // Use JsonSchema.Net's FromText for deserialization
                    JsonSchema? schema = JsonSchema.FromText(schemaJson);
                    if (schema != null)
                    {
                        return schema;
                    }
                    // Console.WriteLine($"Warning: Failed to deserialize cached schema from {schemaPath}. Regenerating."); // Use logger
                }
                catch (Exception)
                {
                    // Console.WriteLine($"Warning: Error reading cached schema from {schemaPath}. Regenerating. Error: {ex.Message}"); // Use logger
                    // Attempt to delete the corrupted cache file
                    try { File.Delete(schemaPath); } catch { /* Ignore delete error */ }
                }
            }
#endif

            // Generate new schema using JsonSchema.Net.Generation
            JsonSchema? outputSchema = schema switch
            {
                EnInferenceSchema.Ollama => OllamaOutputSchema.BuildSchema(),
                EnInferenceSchema.OpenAI => OpenAIOutputSchema.BuildSchema(),
                EnInferenceSchema.Outlines => OutlinesOutputSchema.BuildSchema(),
                EnInferenceSchema.LMFormatEnforcer => null,
                _ => null
            };

            SchemaGeneratorConfiguration config = DefaultSchemaGeneratorConfiguration.DeepClone();
            switch ( schema )
            {
                case EnInferenceSchema.Ollama:
                    config.Refiners.Add(new EnumTypeRefiner());
                    break;
                case EnInferenceSchema.OpenAI:
                    config.Refiners.Add(new EnumTypeRefiner());
                    break;
                case EnInferenceSchema.Outlines:
                    config.Refiners.Add(new EnumTypeRefiner());
                    config.Refiners.Add(new MultipleTypesRefiner());
                    break;
                case EnInferenceSchema.LMFormatEnforcer:
                    config.Refiners.Add(new EnumTypeRefiner());
                    config.Refiners.Add(new MultipleTypesRefiner());
                    break;
                default:
                    throw new ArgumentException($"Unsupported schema type: {schema}");
            }

            JsonSchema? generatedSchema = null;
            try
            {
                JsonSchemaBuilder schemaBuilder = new();
                if ( outputSchema is not null )
                {
                    SchemaRegistry.Global.Register(outputSchema);
                    schemaBuilder = schemaBuilder.Schema(outputSchema.BaseUri);
                }
                schemaBuilder = schemaBuilder.FromType(type, config);

                generatedSchema = schemaBuilder.Build();
                generatedSchema = generatedSchema.InlineReferences();
                if ( outputSchema is not null )
                {
                    generatedSchema = generatedSchema.TrimUnsupported(outputSchema);
                    // generatedSchema = generatedSchema.RemoveSchemaKey();
                }

                // DEBUG: Show the generated schema
                // generatedSchema.ToJson().DisplayAsJson(Color.BlueViolet);
            }
            catch ( Exception ex )
            {
                Console.WriteLine($"Error: Failed to generate schema for {type.Name}. Error: {ex.Message}"); // Use logger
                throw;
            }

            // Save schema to cache
            try
            {
                // Use System.Text.Json for serializing the JsonSchema.Net object
                string schemaJsonOutput = SerializeToJson(generatedSchema);
                await File.WriteAllTextAsync(schemaPath, schemaJsonOutput);
                // Console.WriteLine($"Saved schema to {schemaPath}"); // Use logger
            }
            catch ( Exception )
            {
                // Console.WriteLine($"Error: Failed to save schema to {schemaPath}. Error: {ex.Message}"); // Use logger
                // Optionally re-throw or handle the error appropriately
            }

            // Return the generated schema
            return generatedSchema;
        }

        /// <summary>
        /// Computes a SHA256 hash based on the structure of the given type.
        /// </summary>
        /// <param name="type">The type to hash.</param>
        /// <returns>A hexadecimal string representation of the type's hash.</returns>
        public static string ComputeTypeHash(Type type)
        {
            // Create a string representation of the type structure
            StringBuilder sb = new();
            sb = sb.Append(type.FullName);

            // Include all public instance properties, ordered by name
            foreach ( PropertyInfo property in type.GetProperties(TypeConstants.DefaultBindingFlags)
                .OrderBy(p => p.Name) ) // Ensure consistent ordering
            {
                sb = sb.Append(property.Name);
                sb = sb.Append(property.PropertyType.FullName);
                // Consider adding attributes relevant to schema generation if needed
            }

            // Include public nested types, ordered by name
            if ( type.IsClass || type.IsValueType )
            {
                foreach ( Type nestedType in type.GetNestedTypes(TypeConstants.DefaultBindingFlags)
                    .OrderBy(t => t.Name) )
                {
                    // Recursively compute hash for nested types
                    sb = sb.Append(ComputeTypeHash(nestedType));
                }
            }

            // Compute SHA256 hash of the string representation
            return sb.ToString().ToSha256Hash();
        }

        /// <summary>
        /// Deserializes a JSON string to an object of the specified type using System.Text.Json.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="targetType">The type of the object to deserialize to.</param>
        /// <returns>The deserialized object, or null if deserialization fails or the JSON represents null.</returns>
        /// <exception cref="InvalidOperationException">Thrown if deserialization results in null.</exception>
        public static object DeserializeJsonToType(string json, Type targetType)
        {
            // Deserialize with the configured options and target type
            return JsonSerializer.Deserialize(json, targetType, DefaultJsonSerializerOptions)
                ?? throw new InvalidOperationException(
                    $"Failed to deserialize JSON to type {targetType.Name}. Result was null.");
        }

        public static object DeserializeJsonDocumentToType(JsonDocument jsonDocument, Type targetType)
        {
            // Deserialize with the configured options and target type
            return JsonSerializer.Deserialize(jsonDocument.RootElement.GetRawText(), targetType, DefaultJsonSerializerOptions)
                ?? throw new InvalidOperationException(
                    $"Failed to deserialize JSON to type {targetType.Name}. Result was null.");
        }

        /// <summary>
        /// Deserializes a JSON string to an object of type T using System.Text.Json
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if deserialization results in null.</exception>
        public static T DeserializeJsonToType<T>(string json)
        {
            // Deserialize with the configured options and target type
            return JsonSerializer.Deserialize<T>(json, DefaultJsonSerializerOptions)
            ?? throw new InvalidOperationException(
                $"Failed to deserialize JSON to type {typeof(T).Name}. Result was null.");
        }

        /// <summary>
        /// Gets the primary JSON schema value handling type for the specified C# type.
        /// </summary>
        /// <param name="type">The C# type.</param>
        /// <returns>A task representing the asynchronous operation, containing the <see cref="EnSchemaOutput"/> representing the root type(s) of the generated JSON schema.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the schema type cannot be determined.</exception>
        public static async Task<EnSchemaOutput> GetItemSchemaHandlingTypeAsync(Type type)
        {
            JsonSchema schema = await GetOrCreateJsonSchemaAsync(type);
            SchemaValueType? schemaValueType = schema.GetJsonType();

            if ( schemaValueType != null )
            {
                return schemaValueType.Value.GetOutputHandlingType();
            }

            // Fallback checks if 'type' keyword is missing
            if ( schema.TryGetKeyword<EnumKeyword>(EnumKeyword.Name, out _) )
            {
                // Schemas defined by 'enum' represent a fixed set of values.
                return EnSchemaOutput.ValueType;
            }

            // Fallback or error handling if type and enum keywords are missing.
            // This might happen for complex schemas (e.g., using oneOf, anyOf) or empty schemas.
            // For schemas generated from simple C# types, a TypeKeyword should generally exist.
            // Consider if other keywords like `EnumKeyword` imply `String` or `Number` etc.
            // For now, throw an exception if the type cannot be determined directly.
            throw new InvalidOperationException($"Could not determine the primary SchemaValueType for type {type.FullName}. The generated schema might be complex or lack a 'type' keyword.");
        }
    }
}
