// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Persistence.Entities.Common;

namespace RepoTool.Persistence.Entities
{
    public class LanguageEntity : BaseEntity
    {
        public required string Name { get; set; }
        public required List<string> Patterns { get; set; }
    }
}