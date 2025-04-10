using RepoTool.Attributes;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Builders.Iterable;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Enums.Parser.Tools.Builders
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
}