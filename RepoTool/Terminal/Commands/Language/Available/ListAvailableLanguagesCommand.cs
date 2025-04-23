// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Helpers;
using RepoTool.Models.Resources;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Language.Available
{
    public class ListAvailableLanguagesCommand : Command<ListLanguageSettings>
    {
        public ListAvailableLanguagesCommand()
        {
        }

        public override int Execute(CommandContext context, ListLanguageSettings settings)
        {
            string languagesJson = ResourceHelper.GetParserLanguagesJson();
            List<LanguageEntry> languages = JsonHelper.DeserializeJsonToType<List<LanguageEntry>>(languagesJson)
                ?? throw new InvalidOperationException("Failed to deserialize languages JSON or the resource contained null.");

            Table table = new Table()
                .Title("Available Languages")
                .AddColumn("Name")
                .AddColumn("File Extensions/Patterns");

            if ( languages.Count == 0 )
            {
                AnsiConsole.MarkupLine("[yellow]No available languages found in the configuration.[/]");
                return 0;
            }

            foreach ( LanguageEntry language in languages )
            {
                table = table.AddRow(language.Name, string.Join(", ", language.Patterns));
            }

            AnsiConsole.Write(table);

            return 0;
        }
    }
}
