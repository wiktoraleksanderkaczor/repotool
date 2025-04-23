// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Parser.Tools.Builders.Common
{
    internal enum EnItemTerminationReason
    {
        /// <summary>
        /// Indicates that the iterable has been terminated due to a parsing error.
        /// </summary>
        Mistake,

        /// <summary>
        /// Indicates that the iterable has been terminated due to reaching the end of the iterable.
        /// </summary>
        Finished
    }

    internal sealed record EndItem
    {
        /// <summary>
        /// Indicates why the end of the item has been reached.
        /// </summary>
        public required EnItemTerminationReason TerminationReason { get; init; }
    }
}
