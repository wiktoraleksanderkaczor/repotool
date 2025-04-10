using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <inheritdoc />
    public record VariableUseExpression : ExpressionConstruct
    {
        /// <summary>
        /// Name of the variable.
        /// </summary>
        /// <example>
        /// <code>
        /// myVariable
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "VariableName": "myVariable"
        /// }
        /// </code>
        /// </example>
        public required string VariableName { get; init; }

        /// <summary>
        /// List of components of the variable path (e.g. a.b.c or a[0].b.c).
        /// If the variable is not a path-supported type, then this will be null.
        /// Does not include the initial variable name.
        /// </summary>
        /// <example>
        /// <code>
        /// myObject.myField
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "VariableName": "myObject",
        ///   "Path": [ { "Value": "myField" } ]
        /// }
        /// </code>
        /// </example>
        public required List<VariablePathComponent>? Path { get; init; }
    }

    /// <summary>
    /// Represents a component of a variable path.
    /// </summary>
    public record VariablePathComponent
    {
        /// <summary>
        /// The value of this path component.
        /// </summary>
        /// <example>
        /// <code>
        /// myObject.myField
        /// </code>
        /// Would have a component parsed as:
        /// <code>
        /// {
        ///    "Value": "myField"
        /// }
        /// </code>
        /// </example>
        public required string Value { get; init; }
        
        /// <summary>
        /// The type information for this component.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }
    }
}