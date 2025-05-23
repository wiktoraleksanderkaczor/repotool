// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Models.Documentation.Common
{
    internal abstract record BaseDocumentation
    {
        /// <summary>
        /// Summary block documentation for the item. Can be null if not present.
        /// </summary>
        public required string? Summary { get; set; }

        /// <summary>
        /// Remarks block documentation for the item. Can be null if not present.
        /// </summary>
        public required string? Remarks { get; set; }

        /// <summary>
        /// Example block documentation for the item. Can be null if not present.
        /// </summary>
        public required string? Example { get; set; }
    }

    internal abstract record BaseTypedDocumentation : BaseDocumentation
    {
        /// <summary>
        /// Full name of the type, including namespace and generic types.
        /// </summary>
        public required string TypeName { get; set; }
    }
}
