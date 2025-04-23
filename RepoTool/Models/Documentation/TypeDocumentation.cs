// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Documentation.Common;

namespace RepoTool.Models.Documentation
{
    /// <summary>
    /// Represents the documentation details for a specific member (field or property).
    /// </summary>
    internal sealed record MemberDocumentation : BaseTypedDocumentation
    {
        /// <summary>
        /// Name of the member.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Value of the member. Null if not applicable.
        /// Applicable to enum values
        /// </summary>
        public required string? Value { get; set; }
    }

    internal sealed record StructDocumentation : BaseTypedDocumentation
    {
        public required List<MemberDocumentation>? Fields { get; set; }
    }

    /// <summary>
    /// Represents the structured XML documentation extracted for a specific type.
    /// </summary>
    internal sealed record TypeDocumentation : BaseTypedDocumentation
    {
        /// <summary>
        /// List of types that this type derives from.
        /// </summary>
        public required List<string>? DerivesFrom { get; set; }

        /// <summary>
        /// Boolean for whether type is abstract
        /// </summary>
        public required bool IsAbstract { get; set; }

        /// <summary>
        /// Boolean for whether type is an interface
        /// </summary>
        public required bool IsInterface { get; set; }

        /// <summary>
        /// List of documentation details for the type's fields.
        /// </summary>
        public required List<MemberDocumentation>? Fields { get; set; }

        /// <summary>
        /// List of documentation details for the type's properties.
        /// </summary>
        public required List<MemberDocumentation>? Properties { get; set; }

        /// <summary>
        /// Generic type parameters for the type, if any.
        /// </summary>
        public required List<TypeDocumentation>? Generics { get; set; }

        /// <summary>
        /// List of documentation for types that derive from this type.
        /// This is populated only for abstract classes and interfaces.
        /// </summary>
        public required List<TypeDocumentation>? DerivedTypes { get; set; }

        /// <summary>
        /// List of documentation details for the type's properties' structs.
        /// </summary>
        public required List<StructDocumentation>? Structs { get; set; }
    }
}
