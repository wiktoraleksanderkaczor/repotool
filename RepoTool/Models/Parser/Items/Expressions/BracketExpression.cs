using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <summary>
    /// Defines the possible types of brackets.
    /// </summary>
    public enum EnBracketType
    {
        /// <summary>
        /// Represents square brackets [].
        /// </summary>
        Square,
        
        /// <summary>
        /// Represents round brackets ().
        /// </summary>
        Round,
        
        /// <summary>
        /// Represents angle brackets &lt;&gt;.
        /// </summary>
        Angle,
        
        /// <summary>
        /// Represents curly brackets {}.
        /// </summary>
        Curly
    }

    /// <inheritdoc />
    public record BracketExpression : ExpressionConstruct
    {
        /// <summary>
        /// Type of the brackets.
        /// </summary>
        /// <example>
        /// <code>
        /// [1, 2, 3]
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "BracketType": "Square"
        /// }
        /// </code>
        /// </example>
        public required EnBracketType BracketType { get; init; }

        /// <summary>
        /// Expression within the brackets.
        /// </summary>
        public required ExpressionConstruct? Expression { get; init; }
    }
}