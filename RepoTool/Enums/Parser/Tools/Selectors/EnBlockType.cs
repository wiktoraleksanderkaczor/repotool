// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Items.Blocks;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Enums.Parser.Tools.Selectors
{
    /// <summary>
    /// Defines the possible types of code blocks.
    /// </summary>
    internal enum EnBlockType
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a namespace block.
        /// </summary>
        [ItemChoice(typeof(NamespaceBlock))]
        Namespace,

        /// <summary>
        /// Represents a class block.
        /// </summary>
        [ItemChoice(typeof(ClassBlock))]
        Class,

        /// <summary>
        /// Represents a struct block.
        /// </summary>
        [ItemChoice(typeof(StructBlock))]
        Struct,

        /// <summary>
        /// Represents an interface block.
        /// </summary>
        [ItemChoice(typeof(InterfaceBlock))]
        Interface,

        /// <summary>
        /// Represents an enum block.
        /// </summary>
        [ItemChoice(typeof(EnumBlock))]
        Enum,

        /// <summary>
        /// Represents a record block.
        /// </summary>
        [ItemChoice(typeof(RecordBlock))]
        Record,

        /// <summary>
        /// Represents a callable block (e.g., method, function).
        /// </summary>
        [ItemChoice(typeof(CallableBlock))]
        Callable
    }
}
