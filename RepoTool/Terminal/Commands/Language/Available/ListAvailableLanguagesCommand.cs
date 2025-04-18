// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Text;
using System.Text.Json;
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
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
            List<LanguageEntry> languages = JsonSerializer.Deserialize<List<LanguageEntry>>(languagesJson, options)
                ?? throw new Exception("Failed to deserialize languages JSON.");

            StringBuilder sb = new();
            sb.AppendLine("Available languages:");
            sb.AppendLine();
            for ( int i = 0; i < languages.Count; i++ )
            {
                sb.AppendLine($"{i + 1}. {languages[i].Name}");
                sb.AppendLine($"  - Extensions: {string.Join(", ", languages[i].Patterns)}");
                sb.AppendLine();
            }

            AnsiConsole.WriteLine(sb.ToString());

            return 0;
        }
    }
}
