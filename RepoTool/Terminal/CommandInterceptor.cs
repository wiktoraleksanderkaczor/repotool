// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Constants;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal
{
    internal sealed class CommandInterceptor : ICommandInterceptor
    {
        // <summary>
        // Intercepts the command settings and performs any necessary init checks.
        // Must be non-static to be called by Spectre.Console.
        // </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The command settings.</param>
        public void Intercept(CommandContext context, CommandSettings settings)
        {
            if ( context.Name != "init" )
            {
                // Ensure the directory contains a git repo
                if ( !Directory.Exists(PathConstants.GitFolder) )
                {
                    AnsiConsole.WriteLine($"Directory '{PathConstants.CurrentDirectory}' does not contain git repository.");
                    Environment.Exit(1);
                }

                if ( !Directory.Exists(PathConstants.RepoToolFolder)
                    || !File.Exists(PathConstants.DatabasePath) )
                {
                    AnsiConsole.WriteLine("Must run initialization command first");
                    Environment.Exit(1);
                }
            }
        }
    }
}
