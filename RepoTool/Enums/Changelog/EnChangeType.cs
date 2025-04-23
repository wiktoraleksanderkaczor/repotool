// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the type of change
    /// </summary>
    internal enum EnChangeType
    {
        /// <summary>
        /// New feature or functionality
        /// </summary>
        Feature,

        /// <summary>
        /// Fix for a bug or issue
        /// </summary>
        BugFix,

        /// <summary>
        /// Changes that improve code quality without changing functionality
        /// </summary>
        Refactor,

        /// <summary>
        /// Performance improvement changes
        /// </summary>
        Performance,

        /// <summary>
        /// Security related changes
        /// </summary>
        Security,

        /// <summary>
        /// Changes to documentation
        /// </summary>
        Documentation,

        /// <summary>
        /// Changes to dependencies or third-party libraries
        /// </summary>
        Dependencies,

        /// <summary>
        /// Changes to tests or testing infrastructure
        /// </summary>
        Test,

        /// <summary>
        /// Visual or user experience changes that don't add functionality
        /// </summary>
        Style,

        /// <summary>
        /// Changes to build process, CI/CD, or deployment
        /// </summary>
        DevOps,

        /// <summary>
        /// Changes that don't fit into other categories
        /// </summary>
        Other
    }
}
