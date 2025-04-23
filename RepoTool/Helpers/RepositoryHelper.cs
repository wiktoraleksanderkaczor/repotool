// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using LibGit2Sharp;
using RepoTool.Models.Repository;
using Spectre.Console;
using Tree = LibGit2Sharp.Tree;

namespace RepoTool.Helpers
{
    internal sealed class RepositoryHelper
    {
        private Repository _repository { get; init; }

        public RepositoryHelper() => _repository = new Repository(".");

        public static string DownloadRepository(Uri url, string path = ".")
        {
            try
            {
                return Repository.Clone(url.ToString(), path);
            }
            catch ( NameConflictException )
            {
                return path;
            }
            catch ( RecurseSubmodulesException e )
            {
                AnsiConsole.WriteLine($"Failed to clone repository: {e.Message}");
                return string.Empty;
            }
        }

        public List<string> GetSingleParentCommits()
        {
            Branch mainBranch = _repository.Branches.FirstOrDefault(x => x.FriendlyName == "main")
                ?? _repository.Branches.FirstOrDefault(x => x.FriendlyName == "master")
                ?? throw new InvalidOperationException("Main branch not found");

            List<Commit> repoCommits = mainBranch.Commits
                .Where(x => x.Parents.Count() == 1)
                .Reverse()
                .ToList();

            return repoCommits.Select(x => x.Sha).ToList();
        }

        private Tree? ResolveTreeReference(string? reference)
        {
            if ( string.IsNullOrEmpty(reference) )
            {
                return null;
            }

            // Try as branch name first

            Branch? branch = _repository.Branches.FirstOrDefault(b => b.FriendlyName == reference);
            if ( branch != null )
            {
                return branch.Tip.Tree;
            }

            // Try as commit hash
            try
            {
                Commit? commit = _repository.Lookup<Commit>(reference);
                if ( commit != null )
                {
                    return commit.Tree;
                }
            }
            catch ( ArgumentException )
            {
                // Not a valid commit hash
            }

            // Try as tag
            try
            {
                Tag? tag = _repository.Tags.FirstOrDefault(t => t.FriendlyName == reference);
                if ( tag != null && tag.Target is Commit tagCommit )
                {
                    return tagCommit.Tree;
                }
            }
            catch ( ArgumentNullException )
            {
                // Not a valid tag
            }

            return null;
        }

        public async Task<string?> GetFileContentAsync(string filePath, string? reference = null) => reference is not null ? ( GetBlobForCommit(reference, filePath)?.GetContentText() ) : await File.ReadAllTextAsync(filePath).ConfigureAwait(false);

        private Dictionary<string, string?> GetOriginalFileContents(List<PatchEntryChanges> changes)
        {

            return changes.ToDictionary(x => x.Path, x =>
            {
                string path = x.OldPath ?? x.Path;
                try
                {
                    // TODO: Verify if additional cases need to be handled
                    if ( x.Oid is not null
                        && x.Status is not ChangeKind.Renamed
                        && x.Status is not ChangeKind.Ignored
                        && x.Status is not ChangeKind.Untracked )
                    {
                        return _repository.Lookup<Blob>(x.Oid).GetContentText();
                    }
                }
                catch ( NotFoundException )
                {
                    AnsiConsole.WriteLine($"Warning: Original content not found for {path}");
                }

                return null;
            });
        }

        private List<PatchEntryChanges>? GetChangesBetween(string? source, string? target)
        {
            (Tree sourceTree, Tree targetTree) = GetSourceAndTargetTrees(source, target);
            List<PatchEntryChanges> changes = _repository.Diff.Compare<Patch>(sourceTree, targetTree).ToList();
            return changes.Count == 0 ? null : changes;
        }

        private (Tree sourceTree, Tree targetTree) GetSourceAndTargetTrees(string? source, string? target)
        {
            Tree sourceTree;
            Tree targetTree;


            if ( source == null && target == null )
            {

                // Compare current branch to main
                Branch mainBranch = _repository.Branches.FirstOrDefault(x => x.FriendlyName == "main")
                    ?? _repository.Branches.FirstOrDefault(x => x.FriendlyName == "master")
                    ?? throw new InvalidOperationException("Main branch not found");

                Branch currentBranch = _repository.Head;

                if ( currentBranch.FriendlyName == mainBranch.FriendlyName )
                {
                    throw new InvalidOperationException("You are on the main branch, please switch to a feature branch");
                }

                sourceTree = mainBranch.Tip.Tree;
                targetTree = currentBranch.Tip.Tree;
            }
            else
            {
                // Try to resolve source and target as either commit hashes or branch names
                sourceTree = ResolveTreeReference(source) ?? throw new InvalidOperationException($"Could not resolve source: {source}");
                targetTree = ResolveTreeReference(target) ?? throw new InvalidOperationException($"Could not resolve target: {target}");
            }

            return (sourceTree, targetTree);
        }

        public List<SourceChange>? GetSourceChangesBetween(string? source, string? target)
        {
            List<PatchEntryChanges>? changes = GetChangesBetween(source, target);
            if ( changes == null )
            {
                return null;
            }

            Dictionary<string, string> patches = changes.ToDictionary(x => x.Path ?? x.OldPath, x => x.Patch);
            Dictionary<string, string?> originals = GetOriginalFileContents(changes);

            return changes.Select(x => new SourceChange
            {
                OldPath = x.OldPath,
                Path = x.Path,
                PatchContent = patches[x.Path ?? x.OldPath],
                OriginalContent = originals[x.Path ?? x.OldPath],
                IsAdded = x.Status == ChangeKind.Added,
                IsDeleted = x.Status == ChangeKind.Deleted,
                IsCopied = x.Status == ChangeKind.Copied,
                IsRenamed = x.Status == ChangeKind.Renamed
            }).ToList();
        }

        // TODO: Verify if everything below works... especially nullable blob fetch
        public List<SourceChange>? GetSourceChangesForCommit(string commitSha)
        {
            Commit commit = _repository.Lookup<Commit>(commitSha);
            List<SourceChange>? changes = !commit.Parents.Any()
                ? commit.Tree.Select(x => new SourceChange
                {
                    Path = x.Path,
                    PatchContent = _repository.Diff.Compare(null, GetBlobForCommit(commitSha, x.Path)).Patch,
                    OriginalContent = null,
                    IsAdded = true
                }).ToList()
                : GetSourceChangesBetween(commit.Parents.First().Sha, commitSha);

            return changes;
        }

        public Blob? GetBlobForCommit(string commitSha, string path)
        {
            Commit commit = _repository.Lookup<Commit>(commitSha);
            return commit[path]?.Target as Blob;
        }
    }
}
