// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace RepoTool.Schemas.Refiners
{
    internal sealed class AdditionalPropertiesRefiner : ISchemaRefiner
    {
        public bool ShouldRun(SchemaGenerationContextBase context)
        {
            bool isObject = context
                .Intents.OfType<TypeIntent>()
                .Any(t => t.Type == SchemaValueType.Object);

            return isObject;
        }

        public void Run(SchemaGenerationContextBase context) => context.Intents.Add(new AdditionalPropertiesIntent());
    }

    internal sealed class AdditionalPropertiesIntent() : ISchemaKeywordIntent
    {
        public void Apply(JsonSchemaBuilder builder) => builder.AdditionalProperties(false);
    }
}
