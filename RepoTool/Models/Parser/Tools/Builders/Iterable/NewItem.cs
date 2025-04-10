using RepoTool.Enums.Parser.Tools.Builders.Iterable;

namespace RepoTool.Models.Parser.Tools.Builders.Iterable
{
    public record NewItem
    {
        /// <summary>
        /// Indicates whether a new item has been created.
        /// </summary>
        public required EnIterableInsertAt InsertAt { get; init; }
    }
}