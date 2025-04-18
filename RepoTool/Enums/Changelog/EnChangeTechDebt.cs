// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the technical debt impact of a change
    /// </summary>
    public enum EnChangeTechDebt
    {
        /// <summary>
        /// Change reduces existing technical debt
        /// </summary>
        Reduces,

        /// <summary>
        /// Change has no significant impact on technical debt
        /// </summary>
        Neutral,

        /// <summary>
        /// Change introduces some technical debt with justification
        /// </summary>
        Introduces,

        /// <summary>
        /// Change introduces significant technical debt that should be addressed soon
        /// </summary>
        Significant,

        /// <summary>
        /// Change introduces critical technical debt requiring immediate attention
        /// </summary>
        Critical
    }
}