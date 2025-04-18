// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Changelog
{
    /// <summary>
    /// Represents the performance impact of a change
    /// </summary>
    public enum EnChangePerformanceImpact
    {
        /// <summary>
        /// Change significantly improves performance
        /// </summary>
        SignificantImprovement,

        /// <summary>
        /// Change moderately improves performance
        /// </summary>
        MinorImprovement,

        /// <summary>
        /// Change has no measurable impact on performance
        /// </summary>
        Neutral,

        /// <summary>
        /// Change causes minor performance regression
        /// </summary>
        MinorRegression,

        /// <summary>
        /// Change causes significant performance regression
        /// </summary>
        SignificantRegression
    }
}