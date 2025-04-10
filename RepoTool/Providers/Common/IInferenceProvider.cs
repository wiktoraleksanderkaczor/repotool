using Json.Schema;

public interface IInferenceProvider
{
    public Task<string> GetInferenceAsync(List<InferenceMessage> messages, JsonSchema jsonSchema);
}