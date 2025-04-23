// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Builders.Iterable;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Enums.Parser.Tools.Builders
{
    internal enum EnIterableBuilderTool
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
