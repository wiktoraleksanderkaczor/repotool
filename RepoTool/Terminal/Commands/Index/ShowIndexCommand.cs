// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Index
{
    internal sealed class ShowIndexSettings : CommonSettings
    {
        [CommandOption("--show")]
        [Description("Show changelogs.")]
        public bool Show { get; set; }
    }

    internal sealed class ShowIndexCommand : Command<ShowIndexSettings>
    {
        public override int Execute(CommandContext context, ShowIndexSettings settings)
        {
            AnsiConsole.WriteLine("Show index command executed.");
            return 0;
        }
    }
}
