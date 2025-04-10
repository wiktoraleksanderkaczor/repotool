using RepoTool.Attributes;
using RepoTool.Models.Parser.Interfaces;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Models.Parser.Tools.Builders
{
    public enum EnIterableBuilderTool
    {    
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ItemChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a page down action.
        /// </summary>
        [ItemChoice(typeof(PageDown))]
        PageDown,

        /// <summary>
        /// Represents a new item.
        /// </summary>
        [ItemChoice(typeof(NewItem))]
        NewItem,

        /// <summary>
        /// Represents the end of an iterable.
        /// </summary>
        [ItemChoice(typeof(EndItem))]
        EndItem
    }

    public enum EnIterableInsertAt
    {
        /// <summary>
        /// Indicates that the item should be inserted at the end of the iterable.
        /// </summary>
        End,

        /// <summary>
        /// Indicates that the item should be inserted at the beginning of the iterable.
        /// </summary>
        Start
    }

    public record NewItem
    {
        /// <summary>
        /// Indicates whether a new item has been created.
        /// </summary>
        public required EnIterableInsertAt InsertAt { get; init; }
    }

    public record IterableBuilderSelector : IToolSelector<EnIterableBuilderTool>
    {
        public required EnIterableBuilderTool ToolSelection { get; init; }
    }
}