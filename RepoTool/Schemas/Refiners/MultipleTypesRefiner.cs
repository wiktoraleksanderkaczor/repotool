// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;
using RepoTool.Extensions;

namespace RepoTool.Schemas.Refiners
{
    /// <summary>
    /// Refines the schema generation process for types that can represent multiple JSON types
    /// (e.g., string | number, or int? which is integer | null).
    /// It replaces the default <see cref="TypeIntent"/> with an <see cref="AnyOfIntent"/>
    /// containing individual <see cref="TypeIntent"/>s for each possible JSON type.
    /// </summary>
    public class MultipleTypesRefiner : ISchemaRefiner
    {
        /// <summary>
        /// Determines whether this refiner should run based on the generation context.
        /// Runs only if the context contains a <see cref="TypeIntent"/> indicating multiple possible JSON types.
        /// </summary>
        /// <param name="context">The schema generation context.</param>
        /// <returns>
        /// True if the context contains a <see cref="TypeIntent"/> with multiple type flags set, false otherwise.
        /// </returns>
        public bool ShouldRun(SchemaGenerationContextBase context) =>
            // Check if there's a TypeIntent indicating multiple JSON types.
            // HasMultipleFlags() checks if the enum value is not a power of two and not zero.
            context.Intents.OfType<TypeIntent>().Any(intent => intent.Type.HasMultipleFlags());

        /// <summary>
        /// Modifies the schema generation intents for properties representing multiple JSON types.
        /// Replaces the original <see cref="TypeIntent"/> with an <see cref="AnyOfIntent"/>
        /// containing a separate <see cref="TypeIntent"/> for each possible JSON type.
        /// </summary>
        /// <param name="context">The schema generation context.</param>
        public void Run(SchemaGenerationContextBase context)
        {
            // Find the original TypeIntent that indicates multiple types.
            TypeIntent originalTypeIntent = context.Intents
                .OfType<TypeIntent>()
                .First(intent => intent.Type.HasMultipleFlags());

            // Remove the original multi-type intent.
            context.Intents.Remove(originalTypeIntent);

            List<ISchemaKeywordIntent> otherIntents = context.Intents.ToList();

            Dictionary<SchemaValueType, List<ISchemaKeywordIntent>> subschemas = originalTypeIntent.Type.GetFlags()
                .ToDictionary(flag => flag, flag =>
                {
                    List<ISchemaKeywordIntent> typeIntents = [new TypeIntent(flag)];
                    return flag != SchemaValueType.Null
                        ? otherIntents.Concat(typeIntents).ToList()
                        : typeIntents;
                });

            // Create and add subschema intent
            AnyOfIntent anyOfIntent = new(subschemas.Values);
            context.Intents.Add(anyOfIntent);
        }
    }
}