// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Json.Schema;
using RepoTool.Models.Inference;

namespace RepoTool.Providers.Common
{
    internal interface IInferenceProvider
    {
        public Task<string> GetInferenceAsync(List<InferenceMessage> messages, JsonSchema jsonSchema);
    }
}
