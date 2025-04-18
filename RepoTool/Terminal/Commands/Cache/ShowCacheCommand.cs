// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using RepoTool.Extensions;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Cache
{
    public class ShowCacheSettings : CommonSettings
    {
        [CommandOption("--show")]
        [Description("Show cache.")]
        public bool Show { get; set; }
    }

    public class ShowCacheCommand : Command<ShowCacheSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public ShowCacheCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;
        public override int Execute(CommandContext context, ShowCacheSettings settings)
        {
            foreach ( InferenceCacheEntity entity in _dbContext.InferenceCache )
            {
                AnsiConsole.WriteLine(entity.ToJson());
            }
            return 0;
        }
    }
}
