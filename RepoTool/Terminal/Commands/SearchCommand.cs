using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Commands
{
    public class SearchSettings : CommonSettings
    {
        [CommandOption("--query")]
        [Description("Search query.")]
        public string Query { get; set; } = null!;
    }

    public class SearchCommand : Command<SearchSettings>
    {
        public override int Execute(CommandContext context, SearchSettings settings)
        {
            AnsiConsole.WriteLine($"Search command executed with query: {settings.Query}");
            return 0;
        }
    }
}
