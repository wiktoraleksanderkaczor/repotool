using RepoTool.Attributes;
using RepoTool.Models.Parser.Interfaces;
using RepoTool.Models.Parser.Items.Declarations;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Models.Parser.Tools.Selectors
{
    /// <summary>
    /// Defines the possible types of declarations.
    /// </summary>
    public enum EnDeclarationType
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a delegate declaration.
        /// </summary>
        [ItemChoice(typeof(DelegateDeclaration))]
        Delegate,

        /// <summary>
        /// Represents a property declaration.
        /// </summary>
        [ItemChoice(typeof(PropertyDeclaration))]
        Property,

        /// <summary>
        /// Represents a field declaration.
        /// </summary>
        [ItemChoice(typeof(FieldDeclaration))]
        Field,

        /// <summary>
        /// Represents an event declaration.
        /// </summary>
        [ItemChoice(typeof(EventDeclaration))]
        Event
    }

    public record DeclarationSelector : IToolSelector<EnDeclarationType>
    {
        /// <summary>
        /// The type of the declaration.
        /// </summary>
        /// <example>
        /// <code>
        /// public string MyProperty { get; set; }
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ToolSelection": "Property"
        /// }
        /// </code>
        /// </example>
        public required EnDeclarationType ToolSelection { get; init; }
    }
}