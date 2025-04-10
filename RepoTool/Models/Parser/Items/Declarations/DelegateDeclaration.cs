using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Declarations.Common;

namespace RepoTool.Models.Parser.Items.Declarations
{
    /// <inheritdoc />
    public record DelegateDeclaration : DeclarationConstruct
    {
        /// <summary>
        /// Information about the callable delegate.
        /// </summary>
        public required CallableInfoDefinition CallableInfo { get; init; }
    }
}