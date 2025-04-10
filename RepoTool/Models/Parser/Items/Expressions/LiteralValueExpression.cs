using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <summary>
    /// Represents a literal value expression
    /// Only meant to be used for primitives
    /// Objects to be instanciated through callable use
    /// </summary>
    public record LiteralValueExpression : ExpressionConstruct
    {
        /// <summary>
        /// The literal value as a string.
        /// </summary>
        /// <example>
        /// <code>
        /// 123
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "LiteralValue": "123"
        /// }
        /// </code>
        /// </example>
        public required string LiteralValue { get; init; }

        /// <summary>
        /// The type information for the literal value.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }
    }
}