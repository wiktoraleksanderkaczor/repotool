// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Blocks;
using RepoTool.Enums.Parser.Tools.Blocks;
using RepoTool.Enums.Parser.Tools.Common;
using RepoTool.Models.Parser.Items.Blocks.Common;

namespace RepoTool.Models.Parser.Items.Blocks
{
    /// <summary>
    /// Represents information about inheritance for a class or interface.
    /// </summary>
    public record InheritanceInfo
    {
        // TODO: Might need to be path here?
        /// <summary>
        /// The name of the inherited type.
        /// </summary>
        /// <example>
        /// <code>
        /// class MyClass : MyBaseClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "InheritedName": "MyBaseClass"
        /// }
        /// </code>
        /// </example>
        public required string InheritedName { get; init; }

        /// <summary>
        /// The type of inheritance (inheritance or implementation).
        /// </summary>
        /// <example>
        /// <code>
        /// class MyClass : MyBaseClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "InheritanceType": "Inherits"
        /// }
        /// </code>
        /// </example>
        public required EnInheritanceType InheritanceType { get; init; }

        // TODO: Might need to be path here?
        /// <summary>
        /// List of inherited types
        /// </summary>
        public required List<string> InheritsFrom { get; init; }

        /// <summary>
        /// List of inheritance modifiers.
        /// </summary>
        public required List<EnInheritanceModifier> InheritanceModifiers { get; init; }
    }

    /// <inheritdoc />
    public record ClassBlock : BlockConstruct
    {
        /// <summary>
        /// Classes that this class inherits from or implements in case of interfaces.
        /// </summary>
        /// <example>
        /// <code>
        /// class MyClass : MyBaseClass, IMyInterface { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Inheritance": [
        ///         {
        ///             "InheritedName": "MyBaseClass",
        ///             "InheritanceType": "Inherits"
        ///         },
        ///         {
        ///             "InheritedName": "IMyInterface",
        ///             "InheritanceType": "Implements"
        ///         }
        ///     ]
        /// }
        /// </code>
        /// </example>
        public required List<InheritanceInfo> Inheritance { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this class.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// public static class MyClass { }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ClassModifierFlags": ["Static"]
        /// }
        /// </code>
        /// </example>
        public required List<EnClassModifier> ClassModifierFlags { get; init; }
    }
}
