// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Text.Json;
using System.Text.Json.Nodes;
using Json.More;
using Json.Pointer;
using Json.Schema;
using Spectre.Console;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="JsonSchema"/>.
    /// </summary>
    public static class JsonSchemaExtensions
    {
        /// <summary>
        /// Removes the $schema key from the schema if it exists.
        /// This is useful for cleaning up schemas that may have been generated with a specific $schema version
        /// but are intended to be used without that versioning information.
        /// </summary>
        /// <param name="schema">The schema to process.</param>
        /// <returns>A new <see cref="JsonSchema"/> instance without the $schema key, or the original schema if no changes were needed.</returns>
        public static JsonSchema RemoveSchemaKey(this JsonSchema schema)
        {
            // Preserve boolean schema nature if applicable
            if ( schema == JsonSchema.True || schema == JsonSchema.False )
            {
                // Removing $schema from boolean schema doesn't make sense.
                return schema;
            }

            // Remove $schema keyword if present
            JsonDocument newSchema = schema.ToJsonDocument().RemoveAtPointer(JsonPointer.Create("$schema"));
            return JsonSchema.FromText(newSchema.ToJson());
        }

        /// <summary>
        /// Trims unsupported keywords from the target schema based on the provided meta-schema.
        /// This method ensures that the target schema only contains keywords that are supported by the meta-schema.
        /// </summary>
        /// <param name="target">The target schema to be trimmed.</param>
        /// <param name="metaSchema">The meta-schema that defines the supported keywords.</param>
        /// <returns>A new <see cref="JsonSchema"/> instance with unsupported keywords removed, or the original schema if no changes were needed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if target or metaSchema is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the schema cannot be parsed or processed.</exception>
        /// <remarks>
        /// This method recursively traverses the target schema and removes any keywords (and their nested content)
        /// that are not defined or recognized by the provided meta-schema according to its validation rules.
        /// It evaluates the target schema against the meta-schema and identifies invalid parts based on the evaluation results.
        /// </remarks>
        public static JsonSchema TrimUnsupported(this JsonSchema target, JsonSchema metaSchema)
        {
            // Boolean schemas don't have keywords to trim
            if ( target == JsonSchema.True || target == JsonSchema.False )
            {
                return target;
            }

            // Evaluate the target schema against the meta-schema
            // Using Hierarchical output helps pinpoint invalid locations
            EvaluationOptions options = new()
            {
                OutputFormat = OutputFormat.Hierarchical,
                ValidateAgainstMetaSchema = true
            };
            JsonDocument targetDocument = target.ToJsonDocument();
            EvaluationResults evaluationResult = metaSchema.Evaluate(targetDocument, options);

            // If the schema is already valid according to the meta-schema, no trimming is needed.
            if ( evaluationResult.IsValid )
            {
                return target;
            }

            List<EvaluationResults> invalidResults = evaluationResult.GatherErrors();
            invalidResults.DisplayErrors();
            targetDocument.ToJson().DisplayAsJson(Color.Black);
            foreach ( EvaluationResults invalidKeyword in invalidResults )
            {
                // Remove the invalid keyword from the target schema
                JsonPointer invalidPointer = invalidKeyword.InstanceLocation;
                targetDocument = invalidPointer.TryEvaluate(targetDocument.GetAsJsonNode(), out JsonNode? invalidNode)
                    ? targetDocument.RemoveAtPointer(invalidPointer)
                    : throw new InvalidOperationException("Schema is incomplete, cannot trim missing keyword");
            }

            return metaSchema.Evaluate(targetDocument, options).HasErrors
                ? throw new InvalidOperationException("Schema is incomplete after trimming")
                : JsonSchema.FromText(targetDocument.ToJson());
        }

        /// <summary>
        /// Merges the source schema into the target schema, moving all definitions ($defs) to the root level.
        /// If <paramref name="propertyName"/> is provided, the source schema is added as a property under that name.
        /// Otherwise, the properties of the source schema are merged into the target schema's properties.
        /// Returns a new JsonSchema instance representing the merged result.
        /// </summary>
        /// <param name="target">The target schema to merge into.</param>
        /// <param name="source">The source schema to merge.</param>
        /// <param name="propertyName">Optional name to add the source schema as a property.</param>
        /// <returns>A new <see cref="JsonSchema"/> instance with merged content.</returns>
        public static JsonSchema Merge(this JsonSchema target, JsonSchema source, string? propertyName = null)
        {
            // Preserve boolean schema nature if applicable
            if ( target == JsonSchema.True || target == JsonSchema.False )
            {
                // Merging properties or defs into a boolean schema doesn't make sense.
                return target;
            }

            // --- 1. Extract and Combine Definitions ($defs) ---
            Dictionary<string, JsonSchema> mergedDefs = [];

            // Extract from target
            DefsKeyword? targetDefsKeyword = target.Keywords?.OfType<DefsKeyword>().FirstOrDefault();
            if ( targetDefsKeyword != null )
            {
                foreach ( KeyValuePair<string, JsonSchema> def in targetDefsKeyword.Definitions )
                {
                    mergedDefs[def.Key] = def.Value;
                }
            }
            // Support older "definitions" keyword as well
            DefinitionsKeyword? targetDefinitionsKeyword = target.Keywords?.OfType<DefinitionsKeyword>().FirstOrDefault();
            if ( targetDefinitionsKeyword != null )
            {
                foreach ( KeyValuePair<string, JsonSchema> def in targetDefinitionsKeyword.Definitions )
                {
                    // Avoid overwriting if already present from $defs
                    mergedDefs.TryAdd(def.Key, def.Value);
                }
            }


            // Extract from source
            DefsKeyword? sourceDefsKeyword = source.Keywords?.OfType<DefsKeyword>().FirstOrDefault();
            if ( sourceDefsKeyword != null )
            {
                foreach ( KeyValuePair<string, JsonSchema> def in sourceDefsKeyword.Definitions )
                {
                    mergedDefs[def.Key] = def.Value; // Source overwrites target on conflict
                }
            }
            // Support older "definitions" keyword
            DefinitionsKeyword? sourceDefinitionsKeyword = source.Keywords?.OfType<DefinitionsKeyword>().FirstOrDefault();
            if ( sourceDefinitionsKeyword != null )
            {
                foreach ( KeyValuePair<string, JsonSchema> def in sourceDefinitionsKeyword.Definitions )
                {
                    mergedDefs[def.Key] = def.Value; // Source overwrites target on conflict
                }
            }

            // --- 2. Prepare Target Keywords (excluding defs and properties initially) ---
            Dictionary<string, IJsonSchemaKeyword> finalKeywords = target.Keywords?
            .Where(k => k is not DefsKeyword && k is not DefinitionsKeyword && k is not PropertiesKeyword)
            .ToDictionary(k => k.Keyword(), k => k)
            ?? [];

            // --- 3. Merge Properties ---
            Dictionary<string, JsonSchema> mergedProperties = target.Keywords?.OfType<PropertiesKeyword>().FirstOrDefault()?.Properties.ToDictionary()
                                      ?? [];

            if ( !string.IsNullOrEmpty(propertyName) )
            {
                // Add the entire source schema as a property, but strip its defs as they are moved to root
                JsonSchemaBuilder sourceWithoutDefsBuilder = new();
                if ( source.Keywords != null )
                {
                    foreach ( IJsonSchemaKeyword keyword in source.Keywords.Where(k => k is not DefsKeyword && k is not DefinitionsKeyword) )
                    {
                        sourceWithoutDefsBuilder.Add(keyword);
                    }
                }
                mergedProperties[propertyName] = sourceWithoutDefsBuilder.Build();
            }
            else
            {
                // Merge properties from the source schema
                PropertiesKeyword? sourcePropertiesKeyword = source.Keywords?
                    .OfType<PropertiesKeyword>()
                    .FirstOrDefault();

                if ( sourcePropertiesKeyword != null )
                {
                    // Merge source properties into the merged dictionary, overwriting existing ones
                    foreach ( KeyValuePair<string, JsonSchema> property in sourcePropertiesKeyword.Properties )
                    {
                        mergedProperties[property.Key] = property.Value;
                    }
                }
                // If source has no properties keyword, there's nothing to merge at the property level.
            }

            // --- 4. Add Merged Definitions and Properties to Final Keywords ---
            if ( mergedDefs.Count > 0 )
            {
                finalKeywords[DefsKeyword.Name] = new DefsKeyword(mergedDefs);
            }

            if ( mergedProperties.Count > 0 )
            {
                finalKeywords[PropertiesKeyword.Name] = new PropertiesKeyword(mergedProperties);
            }

            // --- 5. Build Final Schema ---
            JsonSchemaBuilder builder = new();
            foreach ( IJsonSchemaKeyword keyword in finalKeywords.Values )
            {
                builder.Add(keyword);
            }

            return builder.Build();
        }

        /// <summary>
        /// Serializes the schema to a JSON string, replacing internal references (`$ref: '#/...'`)
        /// with the content of the referenced definition. Handles potential recursion by not inlining
        /// references that are already being processed in the current chain.
        /// </summary>
        /// <param name="schema">The schema to process.</param>
        /// <returns>A JSON string representation of the schema with internal references inlined.</returns>
        public static JsonSchema InlineReferences(this JsonSchema schema)
        {
            // Preserve boolean schema nature if applicable
            if ( schema == JsonSchema.True || schema == JsonSchema.False )
            {
                // Inlining boolean schema does not make sense 
                return schema;
            }

            // 1. Extract Definitions
            Dictionary<string, JsonSchema> definitions = [];
            DefsKeyword? defsKeyword = schema.Keywords?.OfType<DefsKeyword>().FirstOrDefault();
            if ( defsKeyword != null )
            {
                foreach ( KeyValuePair<string, JsonSchema> def in defsKeyword.Definitions )
                {
                    definitions[def.Key] = def.Value;
                }
            }
            DefinitionsKeyword? definitionsKeyword = schema.Keywords?.OfType<DefinitionsKeyword>().FirstOrDefault();
            if ( definitionsKeyword != null )
            {
                foreach ( KeyValuePair<string, JsonSchema> def in definitionsKeyword.Definitions )
                {
                    definitions.TryAdd(def.Key, def.Value); // Prefer $defs if key exists
                }
            }

            // 2. Serialize to JsonNode
            string initialJson = schema.ToJson();
            JsonNode rootNode = JsonNode.Parse(initialJson)
                ?? throw new InvalidOperationException("Failed to parse JSON node.");


            // 3. Recursively Inline
            HashSet<string> visitedRefs = [];
            JsonNode inlinedNode = InlineNode(rootNode, definitions, visitedRefs)
                ?? throw new InvalidOperationException("Failed to parse JSON node.");


            // 4. Serialize Result
            return JsonSchema.FromText(inlinedNode.ToJson());
        }

        /// <summary>
        /// Recursively traverses a JsonNode, replacing internal $ref pointers with their definitions.
        /// </summary>
        private static JsonNode? InlineNode(
            JsonNode node,
            IReadOnlyDictionary<string, JsonSchema> definitions,
            HashSet<string> visitedRefs)
        {
            if ( node is JsonObject jsonObject )
            {
                // Check for $ref
                if ( jsonObject.TryGetPropertyValue("$ref", out JsonNode? refNode) &&
                    refNode is JsonValue refValue &&
                    refValue.TryGetValue(out string? refString) &&
                    !string.IsNullOrEmpty(refString) )
                {
                    // Check if it's an internal reference we can resolve
                    string? definitionKey = GetInternalDefinitionKey(refString);
                    if ( definitionKey != null )
                    {
                        // Handle recursion: If we are already processing this ref, return the original $ref node
                        if ( !visitedRefs.Add(refString) )
                        {
                            // Already visiting this ref in the current chain, return original to avoid loop
                            return jsonObject.DeepClone();
                        }

                        try
                        {
                            if ( definitions.TryGetValue(definitionKey, out JsonSchema? definitionSchema) )
                            {
                                // Serialize the definition schema and parse it to JsonNode
                                JsonNode definitionNode = JsonNode.Parse(definitionSchema.ToJson())
                                    ?? throw new InvalidOperationException("Failed to parse definition schema.");

                                // Recursively inline references within the definition itself
                                return InlineNode(definitionNode, definitions, visitedRefs);
                            }
                            else
                            {
                                // Definition not found, return the original $ref node
                                return jsonObject.DeepClone();
                            }
                        }
                        finally
                        {
                            // Backtrack: Remove the ref from the visited set for this path
                            visitedRefs.Remove(refString);
                        }
                    }
                    else
                    {
                        // Not an internal reference we handle, return original
                        return jsonObject.DeepClone();
                    }
                }
                else
                {
                    // Not a $ref node, process its children
                    JsonObject newNode = new(jsonObject.Options);
                    foreach ( KeyValuePair<string, JsonNode?> property in jsonObject )
                    {
                        // Don't copy the definitions keyword to the inlined result
                        if ( property.Key.Equals(DefsKeyword.Name, StringComparison.Ordinal) ||
                            property.Key.Equals(DefinitionsKeyword.Name, StringComparison.Ordinal) )
                        {
                            continue;
                        }

                        if ( property.Value is null )
                        {
                            continue;
                        }

                        newNode.Add(property.Key, InlineNode(property.Value, definitions, visitedRefs));
                    }
                    return newNode;
                }
            }
            else if ( node is JsonArray jsonArray )
            {
                JsonArray newArray = new(jsonArray.Options);
                foreach ( JsonNode? item in jsonArray )
                {
                    if ( item is null )
                    {
                        continue;
                    }

                    newArray.Add(InlineNode(item, definitions, visitedRefs));
                }
                return newArray;
            }
            else
            {
                // Value node (string, number, boolean, null), return a clone
                return node.DeepClone();
            }
        }

        /// <summary>
        /// Extracts the definition key if the reference string points to an internal definition.
        /// Handles both '#/$defs/...' and '#/definitions/...'.
        /// </summary>
        /// <param name="refString">The reference string (e.g., "#/$defs/myDef").</param>
        /// <returns>The definition key (e.g., "myDef") or null if it's not a recognized internal definition reference.</returns>
        private static string? GetInternalDefinitionKey(string refString)
        {
            const string defsPrefix = "#/$defs/";
            const string definitionsPrefix = "#/definitions/";

            if ( refString.StartsWith(defsPrefix, StringComparison.Ordinal) )
            {
                return refString[defsPrefix.Length..];
            }
            else if ( refString.StartsWith(definitionsPrefix, StringComparison.Ordinal) )
            {
                return refString[definitionsPrefix.Length..];
            }
            else
            {
                return null; // Not an internal definition reference we handle
            }
        }
    }
}