// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoTool.Persistence.Configuration.Common;
using RepoTool.Persistence.Entities;

namespace RepoTool.Persistence.Configuration
{
    public class InferenceCacheEntityConfiguration : BaseEntityConfiguration<InferenceCacheEntity>
    {
        public override void Configure(EntityTypeBuilder<InferenceCacheEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.PromptHash)
                .IsRequired();

            builder.Property(x => x.OutputType)
                .IsRequired();

            builder.Property(x => x.InferenceProvider)
                .IsRequired();

            builder.Property(x => x.InferenceModel)
                .IsRequired();

            builder.Property(x => x.ResponseContent)
                .IsRequired();

            builder.HasIndex(x => x.PromptHash);
            builder.HasIndex(x => x.OutputType);
            builder.HasIndex(x => x.InferenceProvider);

            // Composite key unique index
            builder.HasIndex(x => new { x.PromptHash, x.OutputType, x.InferenceProvider })
                .IsUnique();
        }
    }
}
