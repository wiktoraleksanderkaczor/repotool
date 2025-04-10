# Issues

## Issue 1: JSON Schema generation for models with invalid key types
### Configuration
```csharp
SchemaGeneratorConfiguration config = new()
{
    // Ensure consistent order
    PropertyOrder = PropertyOrder.AsDeclared,
    Generators = { new CharSchemaGenerator() }
};
```

Defining models for inference only works for JSON Schema generation when they have valid key types.
### Example
```csharp
JsonSerializer.Serialize(
    new JsonSchemaBuilder().FromType(
        typeof(Dictionary<char, string>), config).Build())
```
### Expected
```json
{
  "type": "object",
  "properties": {
    "a": {
      "type": "string"
    }
  }
}
```
### Actual
`string` is completely ignored when the key type is invalid and the output is an array of the invalid key type.
```json
{
    "type": "array",
    "items": {
        "type": "string",
        "pattern": "^[\\x00-\\x7F]$",
        "minLength": 1,
        "maxLength": 1
    }
}
```

### Workaround
Use a valid key type for every model when using collection types. For example, use `Dictionary<string, string>` instead of `Dictionary<char, string>`. This will ensure that the JSON Schema generation works correctly and produces the expected output.