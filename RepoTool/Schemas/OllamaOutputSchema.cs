// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;

namespace RepoTool.Schemas
{
    /// <summary>
    /// Provides functionality to build the JSON Meta-Schema that validates schemas intended for Ollama.
    /// This meta-schema defines the structure and constraints that a JSON schema must adhere to
    /// in order to be compatible with Ollama's structured output feature.
    /// It reflects the capabilities and limitations documented for Ollama's schema processing.
    /// </summary>
    public static class OllamaOutputSchema
    {
        /// <summary>
        /// Creates and configures a <see cref="JsonSchemaBuilder"/> for the Ollama meta-schema.
        /// This builder defines the structure and constraints of a schema compatible with Ollama.
        /// </summary>
        /// <returns>A configured <see cref="JsonSchemaBuilder"/> instance representing the Ollama meta-schema.</returns>
        public static JsonSchemaBuilder CreateSchemaBuilder()
        {
            // Define a self-reference for recursive schema elements
            const string selfRef = "#";

            // Define the allowed types for the 'type' keyword in an Ollama schema
            string[] allowedTypes = ["object", "string", "number", "integer", "boolean", "array", "null"];

            // Define the schema for the 'properties' keyword value
            JsonSchemaBuilder propertiesSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .PropertyNames(new JsonSchemaBuilder().Type(SchemaValueType.String))
                .AdditionalProperties(new JsonSchemaBuilder().Ref(selfRef));

            // Define the schema for the 'required' keyword value
            JsonSchemaBuilder requiredSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Array)
                .Items(new JsonSchemaBuilder().Type(SchemaValueType.String));

            // Define the schema for the 'items' keyword value (can be a single schema or array of schemas)
            JsonSchemaBuilder itemsSchema = new JsonSchemaBuilder()
                .AnyOf(
                    new JsonSchemaBuilder().Ref(selfRef),
                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Items(new JsonSchemaBuilder().Ref(selfRef)),
                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Object)
                        .Properties(
                            ("enum", new JsonSchemaBuilder().Type(SchemaValueType.Array))
                        )
                        .AdditionalProperties(false)
                );

            // Define the schema for 'anyOf' and 'oneOf' keyword values
            JsonSchemaBuilder anyOfOneOfSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Array)
                .MinItems(1)
                .Items(new JsonSchemaBuilder().Ref(selfRef));

            // Define the schema for the '$defs' keyword value
            JsonSchemaBuilder defsSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .PropertyNames(new JsonSchemaBuilder().Type(SchemaValueType.String))
                .AdditionalProperties(new JsonSchemaBuilder().Ref(selfRef));

            // Allow 'type' to be a single string from allowedTypes or an array of strings from allowedTypes
            JsonSchemaBuilder typeSchema = new JsonSchemaBuilder()
                .AnyOf(
                    new JsonSchemaBuilder().Enum(allowedTypes),
                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Items(new JsonSchemaBuilder().Enum(allowedTypes))
                        .MinItems(1) // Must have at least one type
                        .UniqueItems(true) // Types in the array must be unique
                );

            // Start building the main meta-schema
            JsonSchemaBuilder metaSchemaBuilder = new JsonSchemaBuilder()
                .Schema(MetaSchemas.Draft202012Id) // Use Draft 2020-12 meta-schema
                .Id("https://localhost/ollama-structured-output-meta-schema")
                .Title("Ollama Structured Output Meta-Schema")
                .Type(SchemaValueType.Object)
                .Properties(
                    ("$schema", new JsonSchemaBuilder().Const("https://localhost/ollama-structured-output-meta-schema")),
                    ("type", typeSchema),
                    ("properties", propertiesSchema),
                    ("required", requiredSchema),
                    ("enum", new JsonSchemaBuilder().Type(SchemaValueType.Array)),
                    ("anyOf", anyOfOneOfSchema),
                    ("oneOf", anyOfOneOfSchema),
                    ("items", itemsSchema),
                    ("description", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                    ("minimum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                    ("maximum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                    ("exclusiveMinimum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                    ("exclusiveMaximum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                    ("pattern", new JsonSchemaBuilder()
                        .Type(SchemaValueType.String)
                        // Ensure the pattern itself is a valid regex starting with ^ and ending with $
                        .Pattern(@"^\^.*\$$")
                    ),
                    // Ollama requires additionalProperties to be explicitly false for objects
                    ("additionalProperties", new JsonSchemaBuilder().Const(false)),
                    ("$defs", defsSchema),
                    ("$ref", new JsonSchemaBuilder().Type(SchemaValueType.String))
                )
                .Required("type")
                // Disallow any properties not explicitly defined in this meta-schema
                .AdditionalProperties(false);

            // Add conditional logic using AllOf, If, Then, Not
            metaSchemaBuilder.AllOf(
                // Condition 1: If type is 'object'
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Const("object"))))
                    .Then(new JsonSchemaBuilder()
                        // Then 'properties', 'required', and 'additionalProperties' are required
                        .Required("properties", "required", "additionalProperties")
                        // And 'properties' must be an object, 'required' an array, 'additionalProperties' false
                        .Properties(
                            ("properties", new JsonSchemaBuilder().Type(SchemaValueType.Object)),
                            ("required", new JsonSchemaBuilder().Type(SchemaValueType.Array).Items(new JsonSchemaBuilder().Type(SchemaValueType.String))),
                            ("additionalProperties", new JsonSchemaBuilder().Const(false))
                         )
                        // And 'anyOf' or 'oneOf' must not be present
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("anyOf"),
                                new JsonSchemaBuilder().Required("oneOf")
                            )
                        )
                    ),
                // Condition 2: If 'anyOf' or 'oneOf' is present
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder()
                        .AnyOf(
                            new JsonSchemaBuilder().Required("anyOf"),
                            new JsonSchemaBuilder().Required("oneOf")
                        )
                    )
                    .Then(new JsonSchemaBuilder()
                        // Then 'properties' or 'required' must not be present
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("properties"),
                                new JsonSchemaBuilder().Required("required")
                            )
                        )
                    ),
                // Condition 3: If type is 'integer'
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Const("integer"))))
                    .Then(new JsonSchemaBuilder()
                        // Then numeric constraints are allowed
                        .Properties(
                            ("minimum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                            ("maximum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                            ("exclusiveMinimum", new JsonSchemaBuilder().Type(SchemaValueType.Integer)),
                            ("exclusiveMaximum", new JsonSchemaBuilder().Type(SchemaValueType.Integer))
                        )
                    ),
                // Condition 4: If type is NOT 'integer' (and not object, handled above)
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Enum("number", "string", "boolean", "array", "null"))))
                    .Then(new JsonSchemaBuilder()
                        // Then numeric constraints are disallowed
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("minimum"),
                                new JsonSchemaBuilder().Required("maximum"),
                                new JsonSchemaBuilder().Required("exclusiveMinimum"),
                                new JsonSchemaBuilder().Required("exclusiveMaximum")
                            )
                        )
                    )
            );

            return metaSchemaBuilder;
        }

        /// <summary>
        /// Builds the JSON Meta-Schema that defines and validates schemas intended for Ollama.
        /// This schema ensures any provided schema conforms to Ollama's supported structure and constraints.
        /// </summary>
        /// <returns>A <see cref="JsonSchema"/> instance representing the Ollama meta-schema.</returns>
        public static JsonSchema BuildSchema()
        {
            JsonSchemaBuilder schemaBuilder = CreateSchemaBuilder();
            return schemaBuilder.Build();
        }
    }
}