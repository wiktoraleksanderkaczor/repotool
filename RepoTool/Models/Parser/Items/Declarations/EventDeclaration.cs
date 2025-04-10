using RepoTool.Enums.Parser;
using RepoTool.Models.Parser.Items.Declarations.Common;

namespace RepoTool.Models.Parser.Items.Declarations
{
    /// <inheritdoc />
    public record EventDeclaration : DeclarationConstruct
    {
        /// <summary>
        /// The type of the event.
        /// </summary>
        public required TypeInfoDefinition Type { get; init; }

        /// <summary>
        /// The access modifiers for the event.
        /// </summary>
        /// <example>
        /// <code>
        /// public event EventHandler MyEvent;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "AccessModifierFlags": ["Public"]
        /// }
        /// </code>
        /// </example>
        public required List<EnAccessModifierFlag> AccessModifierFlags { get; init; }
    }
}