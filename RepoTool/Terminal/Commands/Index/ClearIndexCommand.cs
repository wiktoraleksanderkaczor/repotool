// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Persistence;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Index
{
    public class ClearIndexSettings : CommonSettings;

    public class ClearIndexCommand : AsyncCommand<ClearIndexSettings>
    {
        private readonly RepoToolDbContext _dbContext;
        public ClearIndexCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;
        public override async Task<int> ExecuteAsync(CommandContext context, ClearIndexSettings settings)
        {
            _dbContext.Changelogs.RemoveRange(_dbContext.Changelogs);
            _ = await _dbContext.SaveChangesAsync();
            AnsiConsole.WriteLine("Changelogs cleared.");
            return 0;
        }
    }
}
