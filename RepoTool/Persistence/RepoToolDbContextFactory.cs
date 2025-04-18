// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RepoTool.Persistence
{
    public class RepoToolDbContextFactory : IDesignTimeDbContextFactory<RepoToolDbContext>
    {
        public RepoToolDbContext CreateDbContext(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string dbPath = Path.Combine(currentDirectory, ".repotool", "database.db");
            if ( !File.Exists(dbPath) )
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath) ?? throw new InvalidOperationException());
                File.Create(dbPath);
            }
            string connectionString = $"Data Source={dbPath}";
            // string connectionString = $"Data Source=:memory:";

            DbContextOptionsBuilder<RepoToolDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlite(connectionString);

            return new RepoToolDbContext(optionsBuilder.Options);
        }
    }
}