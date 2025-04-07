using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Commands
{
    public class StatusSettings : CommonSettings;

    public class StatusCommand : Command<StatusSettings>
    {
        public override int Execute(CommandContext context, StatusSettings settings)
        {
            AnsiConsole.WriteLine("Status command executed.");
            return 0;
        }
    }
}
