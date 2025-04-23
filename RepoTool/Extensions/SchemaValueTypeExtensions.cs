// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;
using RepoTool.Enums.Json;

namespace RepoTool.Extensions
{
    internal static class SchemaValueTypeExtensions
    {
        public static EnSchemaOutput GetOutputHandlingType(this SchemaValueType schemaValueType)
        {
            return schemaValueType switch
            {
                SchemaValueType.Object => EnSchemaOutput.ObjectType,
                SchemaValueType.Array => EnSchemaOutput.IterableType,
                // All primitive JSON types map to Value
                SchemaValueType.Boolean or SchemaValueType.String or SchemaValueType.Number or SchemaValueType.Integer or SchemaValueType.Null => EnSchemaOutput.ValueType,
                _ => throw new InvalidOperationException($"Unknown '{schemaValueType}' SchemaValueType encountered."),
            };
        }
    }
}
