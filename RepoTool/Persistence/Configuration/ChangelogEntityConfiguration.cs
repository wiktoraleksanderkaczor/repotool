using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RepoTool.Persistence.Entities.Configuration
{
    public class ChangelogEntityConfiguration : BaseEntityConfiguration<ChangelogEntity>
    {
        public override void Configure(EntityTypeBuilder<ChangelogEntity> builder)
        {
            base.Configure(builder);

            // Changes to be serialized as JSON
            builder.OwnsMany(x => x.Changes, b =>
            {
                b.Property(x => x.Description)
                    .IsRequired();
                b.Property(x => x.Reason)
                    .IsRequired();
                b.Property(x => x.Importance)
                    .IsRequired();
                b.Property(x => x.Quality)
                    .IsRequired();
                b.Property(x => x.Area)
                    .IsRequired();
                b.Property(x => x.Type)
                    .IsRequired();
                b.Property(x => x.Specialization)
                    .IsRequired();
                b.Property(x => x.TechnicalDebt)
                    .IsRequired();
                b.Property(x => x.PerformanceImpact)
                    .IsRequired();
                b.Property(x => x.Size)
                    .IsRequired();

                b.ToJson();
            });
                
        }
    }
}