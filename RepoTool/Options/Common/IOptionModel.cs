// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Options.Common
{
    public interface IOptionModel
    {
        /// <summary>
        /// Gets the section name for the options model.
        /// </summary>
        public static abstract string Section { get; }
    }
}