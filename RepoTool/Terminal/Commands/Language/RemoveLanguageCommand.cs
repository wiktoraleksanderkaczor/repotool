using Microsoft.EntityFrameworkCore;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace RepoTool.Commands.Language
{
    public class RemoveLanguageSettings : CommonSettings
    {
        [CommandOption("--name")]
        [Description("Language name.")]
        public string Name { get; set; } = null!;
    }

    public class RemoveLanguageCommand : AsyncCommand<RemoveLanguageSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public RemoveLanguageCommand(RepoToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, RemoveLanguageSettings settings)
        {
            LanguageEntity? languageEntity = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Name == settings.Name);
            if (languageEntity != null)
            {
                _dbContext.Languages.Remove(languageEntity);
                await _dbContext.SaveChangesAsync();
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
