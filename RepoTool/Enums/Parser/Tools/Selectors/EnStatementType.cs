// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Items.Statements;
using RepoTool.Models.Parser.Tools.Navigation;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Enums.Parser.Tools.Selectors
{
    /// <summary>
    /// Defines the possible types of statements.
    /// </summary>
    public enum EnStatementType
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents an expression statement.
        /// </summary>
        [ToolChoice(typeof(ExpressionSelector))]
        Expression,

        /// <summary>
        /// Represents a control flow statement.
        /// </summary>
        [ItemChoice(typeof(ControlFlowStatement))]
        ControlFlow,

        /// <summary>
        /// Represents an exception handling statement.
        /// </summary>
        [ItemChoice(typeof(ExceptionHandlingStatement))]
        ExceptionHandling,

        /// <summary>
        /// Represents a marking statement.
        /// </summary>
        [ItemChoice(typeof(MarkingStatement))]
        Marking,

        /// <summary>
        /// Represents a looping statement.
        /// </summary>
        [ItemChoice(typeof(LoopingStatement))]
        Looping,

        /// <summary>
        /// Represents a branching statement.
        /// </summary>
        [ItemChoice(typeof(BranchingStatement))]
        Branching,

        /// <summary>
        /// Represents an import statement.
        /// </summary>
        [ItemChoice(typeof(ImportStatement))]
        Import,

        /// <summary>
        /// Represents an export statement.
        /// </summary>
        [ItemChoice(typeof(ExportStatement))]
        Export,

        /// <summary>
        /// Represents a custom statement.
        /// </summary>
        [ItemChoice(typeof(CustomStatement))]
        Custom,
    }
}
