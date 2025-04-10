using RepoTool.Models.Parser.Items.Directives.Common;

namespace RepoTool.Models.Parser.Items.Directives
{
    /// <summary>
    /// Represents flags for line preprocessor directive modifiers (hidden, default).
    /// </summary>
    public enum EnLineDirectiveModifierFlag
    {
        /// <summary>
        /// Represents setting a specific line number.
        /// </summary>
        SetNumber,

        /// <summary>
        /// Represents the #line hidden directive.
        /// </summary>
        Hidden,
        
        /// <summary>
        /// Represents the #line default directive.
        /// </summary>
        Default,
    }

    /// <summary>
    /// Represents a line preprocessor directive (covers hidden, default and value).
    /// </summary>
    public record LineDirective : DirectiveConstruct
    {
        /// <summary>
        /// The line number specified in the directive, if applicable.
        /// </summary>
        /// <example>
        /// <code>
        /// #line 200
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "DirectiveLineNumber": 200
        /// }
        /// </code>
        /// </example>
        public int? DirectiveLineNumber { get; init; }
        
        /// <summary>
        /// The file path specified in the directive, if applicable.
        /// </summary>
        /// <example>
        /// <code>
        /// #line 200 "custom.cs"
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "FilePath": ["custom.cs"]
        /// }
        /// </code>
        /// </example>
        public List<string>? FilePath { get; init; }

        /// <summary>
        /// The modifier flags for the directive (hidden, default).
        /// </summary>
        /// <example>
        /// <code>
        /// #line hidden
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "ModifierFlags" : ["Hidden"]
        /// }
        /// </code>
        /// </example>
        public List<EnLineDirectiveModifierFlag>? ModifierFlags { get; init; }
    }
}