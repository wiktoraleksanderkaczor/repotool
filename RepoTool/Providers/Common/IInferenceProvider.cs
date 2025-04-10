using Json.Schema;
using RepoTool.Models.Inference;

namespace RepoTool.Providers.Common
{
    public interface IInferenceProvider
    {
        public Task<string> GetInferenceAsync(List<InferenceMessage> messages, JsonSchema jsonSchema);
    }
}
