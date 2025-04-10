using RepoTool.Enums.Parser;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Common
{
    /// <summary>
    /// Represents the definition of a parameter in a callable.
    /// </summary>
    public record ParameterDefinition
    {
        /// <summary>
        /// Type of the parameter.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this parameter.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// void MyMethod(ref int x)
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ParameterModifierFlags": ["ByReference"]
        /// }
        /// </code>
        /// </example>
        public required List<EnParameterModifierFlag> ParameterModifierFlags { get; init; }

        /// <summary>
        /// Default value for optional parameter by expression, if applicable.
        /// </summary>
        public required ExpressionConstruct? DefaultValue { get; init; }
    }
}