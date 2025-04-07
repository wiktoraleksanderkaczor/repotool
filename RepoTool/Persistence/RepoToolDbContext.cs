using Microsoft.EntityFrameworkCore;
using RepoTool.Persistence.Entities;

namespace RepoTool.Persistence 
{
    public class RepoToolDbContext : DbContext
    {
        public DbSet<ChangelogEntity> Changelogs { get; set; }
        public DbSet<InferenceCacheEntity> InferenceCache { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<ParsedFileEntity> ParsedFiles { get; set; }

        public RepoToolDbContext(DbContextOptions<RepoToolDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // // Call base method
            base.OnModelCreating(modelBuilder);

            // Automatically scan and register EntityConfiguration classes
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepoToolDbContext).Assembly);
        }
    }
}