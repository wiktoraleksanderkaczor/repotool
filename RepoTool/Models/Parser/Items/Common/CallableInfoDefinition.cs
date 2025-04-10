using RepoTool.Enums.Parser;

namespace RepoTool.Models.Parser.Items.Common 
{
    /// <summary>
    /// Represents information about a callable, such as a method or function.
    /// </summary>
    public record CallableInfoDefinition
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
        public required List<EnCallableModifierFlag> CallableModifierFlags { get; init; }
    }
}