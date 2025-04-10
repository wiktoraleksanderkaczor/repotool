using Json.Schema;

namespace RepoTool.Schemas
{
    /// <summary>
    /// Provides functionality to build a JSON Schema that validates output structures for OpenAI API responses.
    /// This schema builder follows known constraints and limitations of the OpenAI schema processing.
    /// NOTE: OpenAI's schema-to-grammar conversion has limitations. Unsupported keywords like 'if/then/else',
    /// 'not', and 'uniqueItems' have been removed from this definition to ensure compatibility,
    /// potentially reducing the strictness of the validation compared to a full Draft 7 schema validator.
    /// </summary>
    public static class OpenAiOutputSchema
    {
        /// <summary>
        /// Enum representing the allowed data types in OpenAI output schema.
        /// </summary>
        public enum EnOpenAiType
        {
            /// <summary>
            /// Represents a string type.
            /// </summary>
            String,

            /// <summary>
            /// Represents a numeric (floating-point) type.
            /// </summary>
            Number,

            /// <summary>
            /// Represents an integer type.
            /// </summary>
            Integer,

            /// <summary>
            /// Represents a boolean type.
            /// </summary>
            Boolean,

            /// <summary>
            /// Represents an array type.
            /// </summary>
            Array,

            /// <summary>
            /// Represents an object type.
            /// </summary>
            Object,

            /// <summary>
            /// Represents a null type.
            /// </summary>
            Null
        }

        // Formats supported by the schema based on OpenAI limitations
        private static readonly string[] _stringFormats = ["date", "date-time", "time", "uuid"];

        /// <summary>
        /// Creates and configures a <see cref="JsonSchemaBuilder"/> for the OpenAI schema.
        /// This builder defines the structure and constraints of the schema following OpenAI's requirements.
        /// </summary>
        /// <returns>A configured <see cref="JsonSchemaBuilder"/> instance.</returns>
        public static JsonSchemaBuilder CreateSchemaBuilder()
        {
            // Define a self-reference for recursive schema elements
            const string selfRef = "#";

            JsonSchemaBuilder schemaBuilder = new JsonSchemaBuilder()
                .Schema("http://json-schema.org/draft-07/schema#")
                // .Id("https://localhost/schemas/openai-output-schema")
                // .Title("OpenAI Output Schema (Compatible)")
                // .Description("Schema for validating OpenAI API response formats, adapted for OpenAI's limitations.")

                // Properties are explicitly defined and additionalProperties defaults to false
                // as per the limitations in LIMITS.md
                .AdditionalProperties(false)

                .Type(SchemaValueType.Object)

                .Properties(
                    ("type", new JsonSchemaBuilder()
                        .Type(SchemaValueType.String)
                        .Description("The data type of the schema element.")
                        .Enum("string", "number", "integer", "boolean", "array", "object", "null")
                    ),

                    ("format", new JsonSchemaBuilder()
                        .Type(SchemaValueType.String)
                        .Description("The specific format of the string data type (limited to OpenAI supported formats).")
                        .Enum(_stringFormats)
                    ),

                    ("description", new JsonSchemaBuilder()
                        .Type(SchemaValueType.String)
                        .Description("A human-readable description of the schema element.")
                    ),

                    ("title", new JsonSchemaBuilder()
                        .Type(SchemaValueType.String)
                        .Description("A title for the schema element.")
                    ),

                    ("minimum", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Integer) // OpenAI only supports this for integer
                        .Description("The minimum allowed value (OpenAI only supports this for type: integer).")
                    ),

                    ("maximum", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Integer) // OpenAI only supports this for integer
                        .Description("The maximum allowed value (OpenAI only supports this for type: integer).")
                    ),

                    ("minItems", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Integer)
                        .Description("The minimum number of items for an array.")
                        .Minimum(0)
                    ),

                    ("maxItems", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Integer)
                        .Description("The maximum number of items for an array.")
                        .Minimum(0)
                    ),

                    ("minLength", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Integer)
                        .Description("The minimum length of a string.")
                        .Minimum(0)
                    ),

                    ("maxLength", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Integer)
                        .Description("The maximum length of a string.")
                        .Minimum(0)
                    ),

                    ("pattern", new JsonSchemaBuilder()
                        .Type(SchemaValueType.String)
                        .Description("A regular expression pattern for string validation. Must start with '^' and end with '$'.")
                    ),

                    ("enum", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Description("A list of allowed values.")
                        .MinItems(1)
                        // uniqueItems is not supported by OpenAI
                    ),

                    ("const", new JsonSchemaBuilder()
                        .Description("A constant value.")
                    ),

                    ("items", new JsonSchemaBuilder()
                        .Description("Schema for items in an array.")
                        .Ref(selfRef)
                    ),

                    ("properties", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Object)
                        .Description("Defines the properties of an object.")
                        .AdditionalProperties(new JsonSchemaBuilder().Ref(selfRef))
                    ),

                    ("required", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Description("Required property names for an object.")
                        .Items(new JsonSchemaBuilder().Type(SchemaValueType.String))
                        // uniqueItems is not supported by OpenAI
                    ),

                    ("anyOf", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Description("The value must validate against at least one of the provided schemas.")
                        .Items(new JsonSchemaBuilder().Ref(selfRef))
                        .MinItems(1)
                    ),

                    ("allOf", new JsonSchemaBuilder()
                        .Type(SchemaValueType.Array)
                        .Description("The value must validate against all of the provided schemas.")
                        .Items(new JsonSchemaBuilder().Ref(selfRef))
                        .MinItems(1)
                    )
                );

            // Removed conditional constraints (if/then/else/not) as they are not supported by OpenAI

            return schemaBuilder;
        }

        /// <summary>
        /// Builds the JSON Schema that defines and validates the OpenAI output structure.
        /// This schema ensures any provided output conforms to the supported structure and constraints.
        /// </summary>
        /// <returns>A <see cref="JsonSchema"/> instance for OpenAI output validation.</returns>
        public static JsonSchema BuildSchema()
        {
            JsonSchemaBuilder schemaBuilder = CreateSchemaBuilder();
            return schemaBuilder.Build();
        }
    }
}