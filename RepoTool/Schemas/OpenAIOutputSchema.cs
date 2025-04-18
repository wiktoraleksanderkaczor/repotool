// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;

namespace RepoTool.Schemas
{
    /// <summary>
    /// Provides functionality to build the JSON Meta-Schema that validates schemas intended for OpenAI Structured Outputs.
    /// This meta-schema defines the structure and constraints that a JSON schema must adhere to
    /// in order to be compatible with OpenAI's structured output feature.
    /// It reflects the capabilities and limitations documented for OpenAI's schema processing.
    /// </summary>
    public static class OpenAIOutputSchema
    {
        /// <summary>
        /// Creates and configures a <see cref="JsonSchemaBuilder"/> for the OpenAI meta-schema.
        /// This builder defines the structure and constraints of a schema compatible with OpenAI Structured Outputs.
        /// </summary>
        /// <returns>A configured <see cref="JsonSchemaBuilder"/> instance representing the OpenAI meta-schema.</returns>
        public static JsonSchemaBuilder CreateSchemaBuilder()
        {
            // Define a self-reference for recursive schema elements
            const string selfRef = "#";

            // Define the allowed types for the 'type' keyword in an OpenAI schema
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

            // Allow 'type' to be a single string from allowedTypes or an array of strings from allowedTypes
            JsonSchemaBuilder typeSchema = new JsonSchemaBuilder()
                .AnyOf(
                    new JsonSchemaBuilder().Enum(allowedTypes),
                    new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Items(new JsonSchemaBuilder().Enum(allowedTypes))
                        .MinItems(1)
                        .UniqueItems(true)
                );

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
                            ("type", typeSchema),
                            ("enum", new JsonSchemaBuilder().Type(SchemaValueType.Array))
                        )
                        .AdditionalProperties(false)
                );

            // Define the schema for 'anyOf' keyword values
            JsonSchemaBuilder anyOfSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Array)
                .MinItems(1)
                .Items(new JsonSchemaBuilder().Ref(selfRef));

            // Define the schema for the '$defs' keyword value
            JsonSchemaBuilder defsSchema = new JsonSchemaBuilder()
                .Type(SchemaValueType.Object)
                .PropertyNames(new JsonSchemaBuilder().Type(SchemaValueType.String))
                .AdditionalProperties(new JsonSchemaBuilder().Ref(selfRef));


            // Start building the main meta-schema
            JsonSchemaBuilder metaSchemaBuilder = new JsonSchemaBuilder()
                .Schema(MetaSchemas.Draft202012Id)
                .Id("https://localhost/openai-structured-output-meta-schema")
                .Title("OpenAI Structured Output Meta-Schema")
                .Type(SchemaValueType.Object)
                .Properties(
                    ("$schema", new JsonSchemaBuilder().Const("https://localhost/openai-structured-output-meta-schema")),
                    ("type", typeSchema),
                    ("properties", propertiesSchema),
                    ("required", requiredSchema),
                    ("enum", new JsonSchemaBuilder().Type(SchemaValueType.Array)),
                    ("anyOf", anyOfSchema),
                    ("items", itemsSchema),
                    ("description", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                    ("$defs", defsSchema),
                    ("$ref", new JsonSchemaBuilder().Type(SchemaValueType.String)),
                    // OpenAI requires additionalProperties to be explicitly false for objects
                    ("additionalProperties", new JsonSchemaBuilder().Const(false))
                )
                .Required("type")
                .AdditionalProperties(false);

            // Add conditional logic using AllOf, If, Then, Not
            metaSchemaBuilder.AllOf(
                // Condition 1: Root object must not be anyOf
                new JsonSchemaBuilder()
                    .Not(new JsonSchemaBuilder().Required("anyOf")),
                // Condition 2: If type is 'object'
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
                    ),
                // Condition 3: If 'anyOf' is present, each schema in anyOf must be valid
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Required("anyOf"))
                    .Then(new JsonSchemaBuilder()
                        .Properties(
                            ("anyOf", new JsonSchemaBuilder()
                                .Type(SchemaValueType.Array)
                                .MinItems(1)
                                .Items(new JsonSchemaBuilder().Ref(selfRef))
                            )
                        )
                    ),
                // Condition 4: All fields must be required (if properties is present, required must include all property names)
                // This is not strictly enforceable in meta-schema, but we require 'required' to be present if 'properties' is present
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Required("properties"))
                    .Then(new JsonSchemaBuilder().Required("required")),
                // Condition 5: If type is 'object', disallow unsupported keywords for objects
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Const("object"))))
                    .Then(new JsonSchemaBuilder()
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("patternProperties"),
                                new JsonSchemaBuilder().Required("unevaluatedProperties"),
                                new JsonSchemaBuilder().Required("propertyNames"),
                                new JsonSchemaBuilder().Required("minProperties"),
                                new JsonSchemaBuilder().Required("maxProperties")
                            )
                        )
                    ),
                // Condition 6: If type is 'string', disallow unsupported keywords for strings
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Const("string"))))
                    .Then(new JsonSchemaBuilder()
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("minLength"),
                                new JsonSchemaBuilder().Required("maxLength"),
                                new JsonSchemaBuilder().Required("pattern"),
                                new JsonSchemaBuilder().Required("format")
                            )
                        )
                    ),
                // Condition 7: If type is 'number' or 'integer', disallow unsupported keywords for numbers
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Enum("number", "integer"))))
                    .Then(new JsonSchemaBuilder()
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("minimum"),
                                new JsonSchemaBuilder().Required("maximum"),
                                new JsonSchemaBuilder().Required("multipleOf")
                            )
                        )
                    ),
                // Condition 8: If type is 'array', disallow unsupported keywords for arrays
                new JsonSchemaBuilder()
                    .If(new JsonSchemaBuilder().Properties(("type", new JsonSchemaBuilder().Const("array"))))
                    .Then(new JsonSchemaBuilder()
                        .Not(new JsonSchemaBuilder()
                            .AnyOf(
                                new JsonSchemaBuilder().Required("unevaluatedItems"),
                                new JsonSchemaBuilder().Required("contains"),
                                new JsonSchemaBuilder().Required("minContains"),
                                new JsonSchemaBuilder().Required("maxContains"),
                                new JsonSchemaBuilder().Required("minItems"),
                                new JsonSchemaBuilder().Required("maxItems"),
                                new JsonSchemaBuilder().Required("uniqueItems")
                            )
                        )
                    )
            );

            return metaSchemaBuilder;
        }

        /// <summary>
        /// Builds the JSON Meta-Schema that defines and validates schemas intended for OpenAI Structured Outputs.
        /// This schema ensures any provided schema conforms to OpenAI's supported structure and constraints.
        /// </summary>
        /// <returns>A <see cref="JsonSchema"/> instance representing the OpenAI meta-schema.</returns>
        public static JsonSchema BuildSchema()
        {
            JsonSchemaBuilder schemaBuilder = CreateSchemaBuilder();
            return schemaBuilder.Build();
        }
    }
}
