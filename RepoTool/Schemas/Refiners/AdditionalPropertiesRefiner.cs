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
            // ⬇️ This is where the magic is. No need to inherit from the base class everywhere.
            // bool isFromCurrentAssembly = context.Type.Assembly == Assembly.GetExecutingAssembly(); 
            return isObject; // && isFromCurrentAssembly;
        }

        public void Run(SchemaGenerationContextBase context) => context.Intents.Add(new AdditionalPropertiesIntent());
    }

    public class AdditionalPropertiesIntent() : ISchemaKeywordIntent
    {
        public void Apply(JsonSchemaBuilder builder) => builder.AdditionalProperties(false);
    }
}