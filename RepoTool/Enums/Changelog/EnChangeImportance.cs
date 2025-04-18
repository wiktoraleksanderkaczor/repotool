// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the importance level of a change
    /// </summary>
    public enum EnChangeImportance
    {
        /// <summary>
        /// Minor change with minimal impact (formatting, comments, etc.)
        /// </summary>
        Minor,

        /// <summary>
        /// Normal change with moderate impact (bug fixes, small features)
        /// </summary>
        Normal,

        /// <summary>
        /// Major change with significant impact (new features, breaking changes)
        /// </summary>
        Major,

        /// <summary>
        /// Critical change that affects core functionality or security
        /// </summary>
        Critical
    }
}