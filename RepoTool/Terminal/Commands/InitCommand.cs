using Microsoft.EntityFrameworkCore;
using RepoTool.Constants;
using RepoTool.Persistence;
using Spectre.Console.Cli;

public class InitCommand : Command<CommonSettings>
{
    private readonly RepoToolDbContext _dbContext;
    public InitCommand(RepoToolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override int Execute(CommandContext context, CommonSettings settings)
    {
        // Ensure containing directory exists
        if (!Directory.Exists(PathConstants.RepoToolFolder))
            Directory.CreateDirectory(PathConstants.RepoToolFolder);

        // Ensure database file exists
        if (!File.Exists(PathConstants.DatabasePath))
            File.Create(PathConstants.DatabasePath).Dispose();

        // Apply migrations
        // Ensure the database is created and migrated
        // dbContext.Database.EnsureCreated() does not work with SQLite    
        _dbContext.Database.Migrate();

        return 0;
    }
}
