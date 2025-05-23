// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Items.Expressions;
using RepoTool.Models.Parser.Tools.Navigation;
using RepoTool.Models.Parser.Tools.Selectors.Expressions;

namespace RepoTool.Enums.Parser.Tools.Selectors
{
    /// <summary>
    /// Defines the possible types of expressions.
    /// </summary>
    internal enum EnExpressionType
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a literal value expression.
        /// </summary>
        [ItemChoice(typeof(LiteralValueExpression))]
        LiteralValue,

        /// <summary>
        /// Represents a variable use expression.
        /// </summary>
        [ItemChoice(typeof(VariableUseExpression))]
        VariableUse,

        /// <summary>
        /// Represents a new variable expression.
        /// </summary>
        [ItemChoice(typeof(NewVariableExpression))]
        NewVariable,

        /// <summary>
        /// Represents a callable use expression.
        /// </summary>
        [ItemChoice(typeof(CallableUseExpression))]
        CallableUse,

        /// <summary>
        /// Represents a bracket expression.
        /// </summary>
        [ItemChoice(typeof(BracketExpression))]
        Bracket,

        /// <summary>
        /// Represents a lambda expression.
        /// </summary>
        [ItemChoice(typeof(LambdaExpression))]
        Lambda,

        /// <summary>
        /// Represents a format string expression.
        /// </summary>
        [ItemChoice(typeof(FormatStringExpression))]
        FormatString,

        /// <summary>
        /// Represents an operator expression selector.
        /// </summary>
        [ToolChoice(typeof(OperatorSelector))]
        Operator,

        /// <summary>
        /// Represents a comprehension expression.
        /// </summary>
        [ItemChoice(typeof(ComprehensionExpression))]
        Comprehension
    }
}
