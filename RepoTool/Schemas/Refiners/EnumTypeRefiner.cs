using System.Text.Json.Nodes;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace RepoTool.Schemas.Refiners
{
    /// <summary>
    /// Refines the schema generation process for enum types to ensure they are represented
    /// as strings with an explicit list of possible values (enum keyword).
    /// </summary>
    public class EnumTypeRefiner : ISchemaRefiner
    {
        /// <summary>
        /// Determines whether this refiner should run based on the generation context.
        /// Runs only if the context type is an enum.
        /// </summary>
        /// <param name="context">The schema generation context.</param>
        /// <returns>True if the context type is an enum, false otherwise.</returns>
        public bool ShouldRun(SchemaGenerationContextBase context)
        {
            // This refiner should only act on types that are explicitly enums.
            return context.Type.IsEnum;
        }

        /// <summary>
        /// Modifies the schema generation intents for an enum type.
        /// It removes default intents and adds a string type intent and an enum keyword
        /// containing the string representations of the enum members.
        /// Handles nullable enums by adding 'null' to the type intent.
        /// </summary>
        /// <param name="context">The schema generation context.</param>
        /// TODO: Change to not clear all intents first
        public void Run(SchemaGenerationContextBase context)
        {
            // Clear existing intents to override default enum handling if necessary.
            // This ensures we precisely control the output format.
            context.Intents.Clear();

            // Determine the base type (string) and handle nullability.
            SchemaValueType type = SchemaValueType.String;
            Type? underlyingType = Nullable.GetUnderlyingType(context.Type);
            if (underlyingType != null)
            {
                // If it's a nullable enum (e.g., MyEnum?), allow null in the schema type.
                type |= SchemaValueType.Null;
            }

            // Add the type intent (string or [string, null]).
            context.Intents.Add(new TypeIntent(type));

            // Get the actual enum type, handling nullable wrappers.
            Type enumType = underlyingType ?? context.Type;

            // Get the string names of the enum members.
            string[] enumNames = Enum.GetNames(enumType);

            // Convert enum names to JsonNode values for the enum keyword.
            JsonNode?[] enumValues = enumNames
                .Select(name => (JsonNode?)JsonValue.Create(name))
                .ToArray();

            // Add the enum keyword with the list of allowed string values.
            context.Intents.Add(new EnumIntent(enumValues));
        }
    }
}