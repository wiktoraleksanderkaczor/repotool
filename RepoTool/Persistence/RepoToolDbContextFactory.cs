// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RepoTool.Persistence
{
    internal sealed class RepoToolDbContextFactory : IDesignTimeDbContextFactory<RepoToolDbContext>
    {
        public RepoToolDbContext CreateDbContext(string[] args)
        {
            string connectionString;
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string dbPath = Path.Combine(currentDirectory, ".repotool", "database.db");
                if ( !File.Exists(dbPath) )
                {
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(dbPath) ?? throw new InvalidOperationException());
                    File.Create(dbPath).Dispose();
                }
                connectionString = $"Data Source={dbPath}";
                // connectionString = $"Data Source=:memory:";
            }
            catch ( Exception ex )
            {
                throw new InvalidOperationException("Could not create the database context.", ex);
            }


            DbContextOptionsBuilder<RepoToolDbContext> optionsBuilder = new();
            optionsBuilder = optionsBuilder.UseSqlite(connectionString);

            return new RepoToolDbContext(optionsBuilder.Options);
        }
    }
}
