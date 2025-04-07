using RepoTool.Helpers;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace RepoTool.Commands.Index
{
    public class UpdateIndexSettings : CommonSettings
    {
        [CommandOption("--commit")]
        [Description("Commit to change.")]
        public string? Commit { get; set; }
    }

    public class UpdateIndexCommand : AsyncCommand<UpdateIndexSettings>
    {
        private readonly RepositoryHelper _repositoryHelper;
        private readonly InferenceHelper _inferenceHelper;
        private readonly RepoToolDbContext _dbContext;

        public UpdateIndexCommand(RepositoryHelper repositoryHelper, InferenceHelper inferenceHelper, RepoToolDbContext dbContext)
        {
            _repositoryHelper = repositoryHelper;
            _inferenceHelper = inferenceHelper;
            _dbContext = dbContext;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, UpdateIndexSettings settings)
        {
            if (settings.Commit != null)
            {
                List<SourceChange>? sourceChanges = _repositoryHelper.GetSourceChangesForCommit(settings.Commit);

                if (sourceChanges != null && sourceChanges.Count != 0)
                {
                    InferenceRequest<ChangelogContext> inferenceRequest = new()
                    {
                        Context = new ChangelogContext
                        {
                            ItemPath = new ItemPath() {
                                Components = [
                                    new ItemPathToolComponent() {
                                        ToolType = typeof(ChangelogEntity),
                                        CurrentObject = null
                                    }
                                ]
                            },
                            SourceChanges = sourceChanges,
                        }
                    };

                    ChangelogEntity? changelog = await _inferenceHelper.GetInferenceAsync<ChangelogEntity, ChangelogContext>(inferenceRequest);

                    if (changelog != null)
                    {
                        _dbContext.Changelogs.Add(changelog);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                List<string> singleParentCommits = _repositoryHelper.GetSingleParentCommits();
                // For each pair of commits, generate a changelog
                for (int i = 0; i < singleParentCommits.Count - 1; i++)
                {
                    List<SourceChange>? sourceChanges = _repositoryHelper.GetSourceChangesBetween(
                        singleParentCommits[i], singleParentCommits[i + 1]);

                    if (sourceChanges != null && sourceChanges.Count != 0)
                    {
                        InferenceRequest<ChangelogContext> inferenceRequest = new()
                        {
                            Context = new ChangelogContext
                            {
                                ItemPath = new ItemPath() {
                                    Components = [
                                        new ItemPathToolComponent() {
                                            ToolType = typeof(ChangelogEntity),
                                            CurrentObject = null
                                        }
                                    ]
                                },
                                SourceChanges = sourceChanges,
                            }
                        };

                        ChangelogEntity? changelog = await _inferenceHelper.GetInferenceAsync<ChangelogEntity, ChangelogContext>(inferenceRequest);

                        if (changelog != null)
                        {
                            _dbContext.Changelogs.Add(changelog);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            return 0;
        }
    }
}
