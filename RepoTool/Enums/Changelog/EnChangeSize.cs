// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the size of a change in a commit, rated against best-practice commit size
    /// </summary>
    internal enum EnChangeSize
    {
        /// <summary>
        /// Excellent - Very small, focused change
        /// </summary>
        Excellent,

        /// <summary>
        /// Good - Small, well-contained change
        /// </summary>
        Good,

        /// <summary>
        /// Acceptable - Medium-sized change, but still focused
        /// </summary>
        Acceptable,

        /// <summary>
        /// Large - Consider refactoring into smaller commits
        /// </summary>
        Large,

        /// <summary>
        /// TooLarge - This change is too large and should be broken down
        /// </summary>
        TooLarge
    }
}
