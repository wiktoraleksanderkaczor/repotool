// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Language
{
    internal sealed class RemoveLanguageSettings : CommonSettings
    {
        [CommandOption("--name")]
        [Description("Language name.")]
        public string Name { get; set; } = null!;
    }

    internal sealed class RemoveLanguageCommand : AsyncCommand<RemoveLanguageSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public RemoveLanguageCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;
        public override async Task<int> ExecuteAsync(CommandContext context, RemoveLanguageSettings settings)
        {
            LanguageEntity? languageEntity = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Name == settings.Name).ConfigureAwait(false);
            if ( languageEntity != null )
            {
                _ = _dbContext.Languages.Remove(languageEntity);
                _ = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                AnsiConsole.WriteLine($"Removed language '{settings.Name}'.");
            }
            else
            {
                AnsiConsole.WriteLine($"Language '{settings.Name}' not found.");
            }
            return 0;
        }
    }
}
