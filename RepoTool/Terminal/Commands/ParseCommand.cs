// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using RepoTool.Helpers;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    public class ParseSettings : CommonSettings
    {
        [CommandArgument(0, "<FILE_PATH>")]
        [Description("Path to the file to parse.")]
        public required string FilePath { get; set; }

        // [CommandOption("--language")]
        // [Description("Language to parse.")]
        // public string? Language { get; set; }
    }

    public class ParseCommand : AsyncCommand<ParseSettings>
    {
        private readonly ParserHelper _parserHelper;

        public ParseCommand(ParserHelper parserHelper) => _parserHelper = parserHelper;

        public override async Task<int> ExecuteAsync(CommandContext context, ParseSettings settings)
        {
            AnsiConsole.WriteLine("Parse file command executed.");
            await _parserHelper.ParseFileAsync(settings.FilePath);
            return await Task.FromResult(0);
        }
    }
}
