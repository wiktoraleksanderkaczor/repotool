
using RepoTool.Enums.Inference;
using RepoTool.Persistence.Entities.Common;

namespace RepoTool.Persistence.Entities
{
    public class InferenceCacheEntity : BaseEntity
    {
        public required string PromptHash { get; set; }
        public required EnInferenceProvider InferenceProvider { get; set; }
        public required string InferenceModel { get; set; }
        public required string OutputType { get; set; }
        public required string ResponseContent { get; set; }
    }
}