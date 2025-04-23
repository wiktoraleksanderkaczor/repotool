// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Flags.Parser
{
    /// <summary>
    /// Defines special flags for JSON schema generation related to properties.
    /// </summary>
    [Flags]
    internal enum JsonSpecialModifier
    {
        /// <summary>
        /// Indicates that no special flags are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Property requires checking over the entire content.
        /// </summary>
        FullContentScan = 1 << 0,

        /// <summary>
        /// Property must be composed of unique items only.
        /// </summary>
        UniqueItems = 1 << 1,
    }
}
