// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Navigation;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Enums.Parser.Tools
{
    /// <summary>
    /// Defines the possible tool selections.
    /// </summary>
    public enum EnConstructType
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a preprocessor directive selection.
        /// </summary>
        [ToolChoice(typeof(DirectiveSelector))]
        PreprocessorDirective,

        /// <summary>
        /// Represents the start of a named block of code
        /// </summary>
        [ToolChoice(typeof(BlockSelector))]
        Block,

        /// <summary>
        /// Represents a statement selection.
        /// </summary>
        [ToolChoice(typeof(StatementSelector))]
        Statement,

        /// <summary>
        /// Represents a declaration selection.
        /// </summary>
        [ToolChoice(typeof(DeclarationSelector))]
        Declaration,

        /// <summary>
        /// Represents a Expression selection.
        /// </summary>
        [ToolChoice(typeof(ExpressionSelector))]
        Expression,

        /// <summary>
        /// Represents the end of a construct.
        /// e.g. end of a class, method, or namespace.
        /// or expression or statement depending on the context.
        /// </summary>
        [ItemChoice(typeof(EndItem))]
        EndConstruct,

        /// <summary>
        /// Represents the finish action.
        /// i.e. completion of the parsing process.
        /// This is the final selection in the parser.
        /// </summary>
        [ItemChoice(typeof(EndItem))]
        Finish,
    }
}
