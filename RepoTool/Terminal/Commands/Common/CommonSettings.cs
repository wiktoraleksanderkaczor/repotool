// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands.Common
{
    internal class CommonSettings : CommandSettings
    {
        [CommandOption("--verbose")]
        [Description("Enable verbose output.")]
        public bool Verbose { get; set; } = false;
    }
}
