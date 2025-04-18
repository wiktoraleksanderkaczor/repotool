// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the functional area affected by a change
    /// </summary>
    public enum EnChangeArea
    {
        /// <summary>
        /// Changes to API endpoints, controllers, or service interfaces
        /// </summary>
        API,

        /// <summary>
        /// Changes to database schema, queries, or data access
        /// </summary>
        Database,

        /// <summary>
        /// Changes to user interface components or styling
        /// </summary>
        UI,

        /// <summary>
        /// Changes to authentication or authorization mechanisms
        /// </summary>
        Security,

        /// <summary>
        /// Changes to performance optimization or system resources
        /// </summary>
        Performance,

        /// <summary>
        /// Changes to configuration or environment settings
        /// </summary>
        Configuration,

        /// <summary>
        /// Changes to documentation or comments
        /// </summary>
        Documentation,

        /// <summary>
        /// Changes to test cases or testing infrastructure
        /// </summary>
        Testing,

        /// <summary>
        /// Changes to build processes, pipeline, or deployment
        /// </summary>
        DevOps,

        /// <summary>
        /// Changes to third-party integrations
        /// </summary>
        Integration,

        /// <summary>
        /// Changes to core business logic or domain model
        /// </summary>
        BusinessLogic,

        /// <summary>
        /// Changes that don't fit into other categories
        /// </summary>
        Other
    }
}