// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using RepoTool.Extensions;
using RepoTool.Helpers;
using RepoTool.Persistence.Entities;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    internal sealed class ParseSettings : CommonSettings
    {
        [CommandArgument(0, "<FILE_PATH>")]
        [Description("Path to the file to parse.")]
        public required string FilePath { get; set; }

        // [CommandOption("--language")]
        // [Description("Language to parse.")]
        // public string? Language { get; set; }
    }

    internal sealed class ParseCommand : AsyncCommand<ParseSettings>
    {
        private readonly ParserHelper _parserHelper;

        public ParseCommand(ParserHelper parserHelper) => _parserHelper = parserHelper;

        public override async Task<int> ExecuteAsync(CommandContext context, ParseSettings settings)
        {
            AnsiConsole.WriteLine("Parse file command executed.");
            ParsedFileEntity entity = await _parserHelper.ParseFileAsync(settings.FilePath).ConfigureAwait(false);
            JsonHelper.SerializeToJson(entity).DisplayAsJson(Color.Green);
            return await Task.FromResult(0).ConfigureAwait(false);
        }
    }
}
