// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Builders.Object;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Enums.Parser.Tools.Builders
{
    public enum EnObjectBuilderTool
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a page down action.
        /// </summary>
        [ItemChoice(typeof(PageDown))]
        PageDown,

        /// <summary>
        /// Represents a new property.
        /// </summary>
        [ItemChoice(typeof(NewProperty))]
        NewProperty,

        /// <summary>
        /// Represents the end of an object.
        /// </summary>
        [ItemChoice(typeof(EndItem))]
        EndItem
    }
}
