// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Expressions;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <summary>
    /// Represents a callable use.
    /// </summary>
    /// <remarks>
    /// Must call expression builder for all [named] arguments
    /// </remarks>
    internal sealed record CallableUseExpression : ExpressionConstruct
    {
        /// <summary>
        /// Name of the function being called.
        /// Null if the object itself is being called.
        /// </summary>
        /// <example>
        /// <code>
        /// myArray.Sort();
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "FunctionName": "Sort"
        /// }
        /// </code>
        /// </example>
        public required string? FunctionName { get; init; }

        /// <summary>
        /// List of arguments for the function, passed as expressions.
        /// </summary>
        public required List<ExpressionConstruct> Arguments { get; init; }

        /// <summary>
        /// Dictionary of named arguments for the function, passed as expressions.
        /// </summary>
        public required Dictionary<string, ExpressionConstruct> NamedArguments { get; init; }

        /// <summary>
        /// Generic parameters for the function.
        /// </summary>
        /// <example>
        /// <code>
        /// MyMethod&lt;string&gt;();
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "GenericParameters": [ { "Name": "string" } ]
        /// }
        /// </code>
        /// </example>
        public required List<TypeInfoDefinition>? GenericParameters { get; init; }

        /// <summary>
        /// Return type of the callable, might be necessary for overloaded callables.
        /// </summary>
        /// <example>
        /// <code>
        /// string result = MyMethod();
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ReturnType": { "Name": "string" }
        /// }
        /// </code>
        /// </example>
        public required TypeInfoDefinition ReturnType { get; init; }

        /// <summary>
        /// Expression used to call the function if applicable.
        /// e.g. a.b.c() or a[0].b.c()
        /// Actual function not included in the variable path
        /// </summary>
        public required ExpressionConstruct? Expression { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this callable.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// // Assuming a built-in function
        /// myArray.Sort();
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "CallableUseModifierFlags": ["IsBuiltIn"]
        /// }
        /// </code>
        /// </example>
        public required List<EnCallableUseModifier> CallableUseModifierFlags { get; init; }
    }
}
