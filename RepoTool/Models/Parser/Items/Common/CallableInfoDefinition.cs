// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Common;

namespace RepoTool.Models.Parser.Items.Common
{
    /// <summary>
    /// Represents information about a callable, such as a method or function.
    /// </summary>
    internal sealed record CallableInfoDefinition
    {
        /// <summary>
        /// Parameters for the callable.
        /// </summary>
        public required List<ParameterDefinition> Parameters { get; init; }

        /// <summary>
        /// Return type of the callable.
        /// It could be void in which case it would be null.
        /// </summary>
        public required TypeInfoDefinition? ReturnType { get; init; }

        /// <summary>
        /// Generic parameters for the callable.
        /// </summary>
        public required List<TypeInfoDefinition>? GenericParameters { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this callable.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// static void MyMethod()
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "CallableModifierFlags": ["Static"]
        /// }
        /// </code>
        /// </example>
        public required List<EnCallableModifier> CallableModifierFlags { get; init; }
    }
}
