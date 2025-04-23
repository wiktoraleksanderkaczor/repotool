// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Language
{
    internal sealed class AddLanguageSettings : CommonSettings
    {
        [CommandArgument(0, "<NAME>")]
        [Description("Language name.")]
        public required string Name { get; set; }

        [CommandArgument(1, "<PATTERNS>")]
        [Description("Language patterns e.g. *.py, *.cs")]
        public required List<string> Patterns { get; set; }
    }

    internal sealed class AddLanguageCommand : AsyncCommand<AddLanguageSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public AddLanguageCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;

        public override async Task<int> ExecuteAsync(CommandContext context, AddLanguageSettings settings)
        {
            LanguageEntity languageEntity = new()
            {
                Name = settings.Name,
                Patterns = settings.Patterns
            };
            _ = _dbContext.Languages.Add(languageEntity);
            _ = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            AnsiConsole.WriteLine($"Added language '{settings.Name}'.");
            return 0;
        }
    }
}
