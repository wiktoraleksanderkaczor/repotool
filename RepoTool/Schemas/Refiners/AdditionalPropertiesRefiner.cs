using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;

namespace RepoTool.Schemas.Refiners
{
    public class AdditionalPropertiesRefiner : ISchemaRefiner
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

    public class AdditionalPropertiesIntent() : ISchemaKeywordIntent
    {
        public void Apply(JsonSchemaBuilder builder) => builder.AdditionalProperties(false);
    }
}