// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RepoTool.Helpers;
using RepoTool.Models.Resources;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Language.Available
{
    public class AddAvailableLanguageSettings : CommonSettings
    {
        [CommandArgument(0, "<NAME>")]
        [Description("Language Name")]
        public required string Name { get; set; }
    }

    public class AddAvailableLanguageCommand : AsyncCommand<AddAvailableLanguageSettings>
    {
        private readonly RepoToolDbContext _dbContext;

        public AddAvailableLanguageCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;

        public override async Task<int> ExecuteAsync(CommandContext context, AddAvailableLanguageSettings settings)
        {
            string languagesJson = ResourceHelper.GetParserLanguagesJson();
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
            List<LanguageEntry> languages = JsonSerializer.Deserialize<List<LanguageEntry>>(languagesJson, options)
                ?? throw new Exception("Failed to deserialize languages JSON.");
            if ( string.IsNullOrEmpty(settings.Name) || !languages.Any(l => l.Name.Equals(settings.Name, StringComparison.OrdinalIgnoreCase)) )
            {
                throw new ArgumentOutOfRangeException(nameof(settings.Name), "Invalid language name.");
            }
            LanguageEntry language = languages.First(l => l.Name.Equals(settings.Name, StringComparison.OrdinalIgnoreCase));

            LanguageEntity? existingLanguage = await _dbContext.Languages
                .FirstOrDefaultAsync(l => l.Name == language.Name);
            if ( existingLanguage != null )
            {
                AnsiConsole.WriteLine($"Language '{language.Name}' already exists.");
                return 0;
            }

            AnsiConsole.WriteLine($"Adding language '{language.Name}'.");
            LanguageEntity languageEntity = new()
            {
                Name = language.Name,
                Patterns = language.Patterns
            };
            _dbContext.Languages.Add(languageEntity);
            await _dbContext.SaveChangesAsync();
            return 0;
        }
    }
}
