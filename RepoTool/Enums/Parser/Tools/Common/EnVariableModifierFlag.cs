// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Tools.Common
{
    public enum EnVariableModifier
    {
        /// <summary>
        /// Indicates that a variable is constant/final and cannot be changed after initialization
        /// </summary>
        Constant,

        /// <summary>
        /// Indicates that a variable is volatile (can be modified by multiple threads)
        /// </summary>
        Volatile
    }
}
