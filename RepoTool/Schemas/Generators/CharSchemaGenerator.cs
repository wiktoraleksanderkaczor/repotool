// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Generators;
using Json.Schema.Generation.Intents;

namespace RepoTool.Schemas.Generators
{
    internal sealed class CharSchemaGenerator : ISchemaGenerator
    {
        public bool Handles(Type type) => type == typeof(char);

        public void AddConstraints(SchemaGenerationContextBase context)
        {
            context.Intents.Add(new TypeIntent(SchemaValueType.String));
            context.Intents.Add(new PatternIntent(@"^[\x00-\x7F]$"));
            context.Intents.Add(new MinLengthIntent(1));
            context.Intents.Add(new MaxLengthIntent(1));
            // context.Intents.Add(new RequiredIntent());
        }
    }
}
