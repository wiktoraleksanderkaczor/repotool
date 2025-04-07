using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RepoTool.Persistence.Entities.Configuration
{
    public class LanguageEntityConfiguration : BaseEntityConfiguration<LanguageEntity>
    {
        public override void Configure(EntityTypeBuilder<LanguageEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .IsRequired();
            builder.Property(x => x.Patterns)
                .IsRequired()
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
                .Metadata.SetValueComparer(
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}