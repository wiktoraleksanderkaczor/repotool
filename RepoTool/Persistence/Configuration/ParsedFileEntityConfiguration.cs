// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoTool.Persistence.Configuration.Common;
using RepoTool.Persistence.Entities;

namespace RepoTool.Persistence.Configuration
{
    public class ParsedFileEntityConfiguration : BaseEntityConfiguration<ParsedFileEntity>
    {
        public override void Configure(EntityTypeBuilder<ParsedFileEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FilePath)
                .IsRequired(false);

            builder.Property(x => x.FileHash)
                .IsRequired(false);

            builder
                .HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ParsedFile)
                .IsRequired(false)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => DeserializeParsedData(v));

            // Add individual indexes
            builder.HasIndex(x => x.FilePath);
            builder.HasIndex(x => x.FileHash);
            builder.HasIndex(x => x.LanguageId);

            // Composite key unique index
            builder.HasIndex(x => new { x.FilePath, x.FileHash })
                .IsUnique();
        }

        private static ParsedData DeserializeParsedData(string json)
        {
            ParsedData? entity = JsonSerializer.Deserialize<ParsedData>(json, (JsonSerializerOptions?)null) ?? throw new InvalidOperationException("Failed to deserialize ParsedData.");
            return entity;
        }
    }
}
