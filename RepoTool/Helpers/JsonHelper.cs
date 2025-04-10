using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Json.Schema;
using Json.Schema.Generation;
using Microsoft.Extensions.AI;
using RepoTool.Constants;
using RepoTool.Extensions;
using RepoTool.Schemas;

namespace RepoTool.Helpers
{
    public enum EnOutputHandlingType
    {
        Object,
        Iterable,
        Value
    }

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
        };

        public static SchemaGeneratorConfiguration DefaultSchemaGeneratorConfiguration { get; } = new()
        {
            // Ensure consistent order
            PropertyOrder = PropertyOrder.AsDeclared,
            // Add custom schema generators if needed
            Generators = { new CharSchemaGenerator() },
            Refiners = { new AdditionalPropertiesRefiner() }
        };

        /// <summary>
        /// Transforms a JSON schema node during creation.
        /// This is a placeholder implementation that returns the node unchanged.
        /// </summary>
        /// <param name="context">The context for schema creation.</param>
        /// <param name="node">The JSON node to transform.</param>
        /// <returns>The transformed JSON node.</returns>
        public static JsonNode DefaultTransformSchemaNode(AIJsonSchemaCreateContext context, JsonNode node)
        {
            // context.TypeInfo.
            // Placeholder: Implement actual transformation logic here if needed.
            // For example, modifying descriptions, adding constraints, etc.
            return node;
        }

        /// <summary>
        /// Serializes an object to a JSON string using System.Text.Json.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string SerializeToJson(object obj)
        {
            // Use System.Text.Json for serializing the object
            return JsonSerializer.Serialize(obj, DefaultJsonSerializerOptions);
        }

        /// <summary>
        /// Gets the JsonSchema object for the specified type, creating and caching it if necessary.
        /// </summary>
        /// <param name="type">The type for which to get the schema object.</param>
        /// <returns>A task representing the asynchronous operation, containing the <see cref="JsonSchema"/> object.</returns>
        public static async Task<JsonSchema> GetOrCreateJsonSchemaAsync(Type type)
        {
            // Create schemas directory if it doesn't exist
            if (!Directory.Exists(PathConstants.UserRepoToolSchemaFolder))
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
            JsonSchemaBuilder schemaBuilder = OllamaOutputSchema.CreateSchemaBuilder().FromType(type, DefaultSchemaGeneratorConfiguration);
            // JsonSchemaBuilder schemaBuilder = new JsonSchemaBuilder().FromType(type, DefaultSchemaGeneratorConfiguration);
            JsonSchema? generatedSchema = null;
            try
            {

                generatedSchema = schemaBuilder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to generate schema for {type.Name}. Error: {ex.Message}"); // Use logger
                throw;
                // Optionally re-throw or handle the error appropriately
            }

            // Save schema to cache
            try
            {
                // Use System.Text.Json for serializing the JsonSchema.Net object
                string schemaJsonOutput = JsonSerializer.Serialize(generatedSchema, DefaultJsonSerializerOptions);
                // string resolved = SchemaResolver.ResolveReferences(schemaJsonOutput);
                // generatedSchema = JsonSchema.FromText(resolved);
                await File.WriteAllTextAsync(schemaPath, schemaJsonOutput);
                // Console.WriteLine($"Saved schema to {schemaPath}"); // Use logger
            }
            catch (Exception)
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
            sb.Append(type.FullName);

            // Include all public instance properties, ordered by name
            foreach (PropertyInfo property in type.GetProperties(TypeConstants.DefaultBindingFlags)
                .OrderBy(p => p.Name)) // Ensure consistent ordering
            {
                sb.Append(property.Name);
                sb.Append(property.PropertyType.FullName);
                // Consider adding attributes relevant to schema generation if needed
            }

            // Include public nested types, ordered by name
            if (type.IsClass || type.IsValueType)
            {
                foreach (Type nestedType in type.GetNestedTypes(TypeConstants.DefaultBindingFlags)
                    .OrderBy(t => t.Name))
                {
                    // Recursively compute hash for nested types
                    sb.Append(ComputeTypeHash(nestedType));
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
        /// <returns>A task representing the asynchronous operation, containing the <see cref="EnOutputHandlingType"/> representing the root type(s) of the generated JSON schema.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the schema type cannot be determined.</exception>
        public static async Task<EnOutputHandlingType> GetItemSchemaHandlingTypeAsync(Type type)
        {
            JsonSchema schema = await GetOrCreateJsonSchemaAsync(type);
            SchemaValueType? schemaValueType = schema.GetJsonType();

            if (schemaValueType != null)
            {
                switch (schemaValueType)
                {
                    case SchemaValueType.Object:
                        return EnOutputHandlingType.Object;
                    case SchemaValueType.Array:
                        return EnOutputHandlingType.Iterable;
                    // All primitive JSON types map to Value
                    case SchemaValueType.Boolean:
                    case SchemaValueType.String:
                    case SchemaValueType.Number:
                    case SchemaValueType.Integer:
                    case SchemaValueType.Null: // Consider if Null should be handled differently
                        return EnOutputHandlingType.Value;
                    default:
                        throw new InvalidOperationException($"Unknown '{schemaValueType}' SchemaValueType encountered.");
                }
            }
            
            // Fallback checks if 'type' keyword is missing
            if (schema.TryGetKeyword<EnumKeyword>(EnumKeyword.Name, out _))
            {
                // Schemas defined by 'enum' represent a fixed set of values.
                return EnOutputHandlingType.Value;
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