using Spectre.Console.Cli;
using RepoTool.Helpers;
using System.ComponentModel;
using Spectre.Console;
using RepoTool.Terminal.Commands.Common;
using RepoTool.Models.Inference.Contexts;
using RepoTool.Models.Inference;
using RepoTool.Models.Inference.Contexts.Parser;

namespace RepoTool.Commands
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

        public SummarizeCommand(InferenceHelper inferenceHelper)
        {
            _inferenceHelper = inferenceHelper;
        }

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