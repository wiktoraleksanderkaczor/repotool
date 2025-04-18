// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Tools.Blocks
{
    public enum EnClassModifierFlag
    {
        /// <summary>
        /// Indicates that a class is partial (can be split across multiple files)
        /// </summary>
        Partial,

        /// <summary>
        /// Indicates that a class is generic
        /// </summary>
        Generic,

        /// <summary>
        /// Indicates that a class is an interface
        /// </summary>
        Interface,

        /// <summary>
        /// Indicates that a class is a attribute
        /// </summary>
        Attribute,
    }
}
