// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    internal sealed class StatusSettings : CommonSettings;

    internal sealed class StatusCommand : Command<StatusSettings>
    {
        public override int Execute(CommandContext context, StatusSettings settings)
        {
            AnsiConsole.WriteLine("Status command executed.");
            return 0;
        }
    }
}
