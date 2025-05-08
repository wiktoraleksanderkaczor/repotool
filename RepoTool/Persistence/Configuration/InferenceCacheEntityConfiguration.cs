// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoTool.Persistence.Configuration.Common;
using RepoTool.Persistence.Entities;

namespace RepoTool.Persistence.Configuration
{
    internal sealed class InferenceCacheEntityConfiguration : BaseEntityConfiguration<InferenceCacheEntity>
    {
        public override void Configure(EntityTypeBuilder<InferenceCacheEntity> builder)
        {
            base.Configure(builder);

            _ = builder.Property(x => x.PromptHash)
                .IsRequired();

            _ = builder.Property(x => x.OutputType)
                .IsRequired();

            _ = builder.Property(x => x.InferenceProvider)
                .IsRequired();

            _ = builder.Property(x => x.InferenceModel)
                .IsRequired();

            _ = builder.Property(x => x.ResponseContent)
                .IsRequired();

            _ = builder.HasIndex(x => x.PromptHash);
            _ = builder.HasIndex(x => x.OutputType);
            _ = builder.HasIndex(x => x.InferenceProvider);

            // Composite key unique index
            _ = builder.HasIndex(x => new { x.PromptHash, x.OutputType, x.InferenceProvider, x.InferenceModel })
                .IsUnique();
        }
    }
}
