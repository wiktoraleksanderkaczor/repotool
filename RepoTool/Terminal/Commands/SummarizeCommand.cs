// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.ComponentModel;
using RepoTool.Helpers;
using RepoTool.Models.Inference;
using RepoTool.Models.Inference.Contexts;
using RepoTool.Models.Inference.Contexts.Parser;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    public class SummarizeSettings : CommonSettings
    {
        [CommandArgument(0, "<FILE_PATH>")]
        [Description("Path to the file to summarize.")]
        public required string FilePath { get; set; }
    }

    public class SummarizeCommand : AsyncCommand<SummarizeSettings>
    {
        private readonly InferenceHelper _inferenceHelper;

        public SummarizeCommand(InferenceHelper inferenceHelper) => _inferenceHelper = inferenceHelper;

        public override async Task<int> ExecuteAsync(CommandContext context, SummarizeSettings settings)
        {
            string fileContent = await File.ReadAllTextAsync(settings.FilePath);

            InferenceRequest<SummarizationContext> request = new()
            {
                Context = new SummarizationContext()
                {
                    ItemPath = new ItemPath()
                    {
                        Components = [
                            new ItemPathToolComponent()
                            {
                                ToolType = typeof(string),
                                CurrentObject = null
                            }
                        ]
                    },
                    Content = fileContent,
                }
            };

            string? summary = await _inferenceHelper.GetInferenceAsync<string, SummarizationContext>(request);
            AnsiConsole.WriteLine(summary ?? "No summary available.");

            return 0;
        }
    }
}
