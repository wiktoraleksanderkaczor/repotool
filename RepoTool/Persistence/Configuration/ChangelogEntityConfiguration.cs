// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoTool.Persistence.Configuration.Common;
using RepoTool.Persistence.Entities;

namespace RepoTool.Persistence.Configuration
{
    public class ChangelogEntityConfiguration : BaseEntityConfiguration<ChangelogEntity>
    {
        public override void Configure(EntityTypeBuilder<ChangelogEntity> builder)
        {
            base.Configure(builder);

            // Changes to be serialized as JSON
            _ = builder.OwnsMany(x => x.Changes, b =>
            {
                _ = b.Property(x => x.Description)
                    .IsRequired();
                _ = b.Property(x => x.Reason)
                    .IsRequired();
                _ = b.Property(x => x.Importance)
                    .IsRequired();
                _ = b.Property(x => x.Quality)
                    .IsRequired();
                _ = b.Property(x => x.Area)
                    .IsRequired();
                _ = b.Property(x => x.Type)
                    .IsRequired();
                _ = b.Property(x => x.Specialization)
                    .IsRequired();
                _ = b.Property(x => x.TechnicalDebt)
                    .IsRequired();
                _ = b.Property(x => x.PerformanceImpact)
                    .IsRequired();
                _ = b.Property(x => x.Size)
                    .IsRequired();

                _ = b.ToJson();
            });

        }
    }
}
