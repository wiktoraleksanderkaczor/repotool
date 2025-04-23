// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Extensions;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Language
{
    internal sealed class ListLanguageSettings : CommonSettings
    {
    }

    internal sealed class ListLanguageCommand : Command<ListLanguageSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public ListLanguageCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;
        public override int Execute(CommandContext context, ListLanguageSettings settings)
        {
            foreach ( LanguageEntity entry in _dbContext.Languages )
            {
                AnsiConsole.WriteLine(entry.ToJson());
            }
            return 0;
        }
    }
}
