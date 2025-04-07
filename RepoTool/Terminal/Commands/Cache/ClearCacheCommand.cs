using System.ComponentModel;
using RepoTool.Persistence;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Commands.Cache
{
    public class ClearCacheSettings : CommonSettings
    {
        [CommandOption("--clear")]
        [Description("Clear cache.")]
        public bool Clear { get; set; }
    }

    public class ClearCacheCommand : AsyncCommand<ClearCacheSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public ClearCacheCommand(RepoToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, ClearCacheSettings settings)
        {
            _dbContext.InferenceCache.RemoveRange(_dbContext.InferenceCache);
            await _dbContext.SaveChangesAsync();
            AnsiConsole.WriteLine("Cache cleared.");
            return 0;
        }
    }
}
