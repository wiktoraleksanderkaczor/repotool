// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoTool.Persistence.Entities.Common;

namespace RepoTool.Persistence.Configuration.Common
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Add primary key
            builder.HasKey(x => x.Id);

            // Add created timestamp with default value
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Add last modified timestamp with default value
            builder.Property(x => x.LastModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Add indexes
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.LastModifiedAt);
        }
    }
}
