// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;
using RepoTool.Enums.Json;

namespace RepoTool.Extensions
{
    public static class SchemaValueTypeExtensions
    {
        public static EnOutputHandlingType GetOutputHandlingType(this SchemaValueType schemaValueType)
        {
            return schemaValueType switch
            {
                SchemaValueType.Object => EnOutputHandlingType.Object,
                SchemaValueType.Array => EnOutputHandlingType.Iterable,
                // All primitive JSON types map to Value
                SchemaValueType.Boolean or SchemaValueType.String or SchemaValueType.Number or SchemaValueType.Integer or SchemaValueType.Null => EnOutputHandlingType.Value,
                _ => throw new InvalidOperationException($"Unknown '{schemaValueType}' SchemaValueType encountered."),
            };
        }
    }
}