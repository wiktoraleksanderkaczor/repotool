using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Commands.Index
{
    public class ShowIndexSettings : CommonSettings
    {
        [CommandOption("--show")]
        [Description("Show changelogs.")]
        public bool Show { get; set; }
    }

    public class ShowIndexCommand : Command<ShowIndexSettings>
    {
        public override int Execute(CommandContext context, ShowIndexSettings settings)
        {
            AnsiConsole.WriteLine("Show index command executed.");
            return 0;
        }
    }
}
