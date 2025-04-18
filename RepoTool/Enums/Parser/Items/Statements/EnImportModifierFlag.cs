// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Items.Statements
{
    /// <summary>
    /// Defines the possible modifier flags for an import statement.
    /// </summary>
    public enum EnImportModifierFlag
    {
        /// <summary>
        /// Import is a system module.
        /// Part of the standard library.
        /// </summary>
        System,

        /// <summary>
        /// Import is a relative path.
        /// </summary>
        Relative,

        /// <summary>
        /// Import is a wildcard import.
        /// </summary>
        Wildcard,
    }
}