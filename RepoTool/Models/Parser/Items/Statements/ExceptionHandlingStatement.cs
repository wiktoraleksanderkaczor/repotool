// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    internal sealed record ExceptionHandlingStatement : StatementConstruct
    {
        /// <summary>
        /// The list of constructs to execute in the try block.
        /// e.g., statements, expressions, etc.
        /// </summary>
        public required List<Construct>? Try { get; init; }

        /// <summary>
        /// The list of catch blocks to handle exceptions.
        /// </summary>
        public required List<CatchComponent> Catches { get; init; }

        /// <summary>
        /// The list of constructs to execute in the finally block.
        /// e.g., statements, expressions, etc.
        /// </summary>
        public required List<Construct>? Finally { get; init; }
    }

    /// <summary>
    /// Represents a catch block in a try-catch-finally statement.
    /// </summary>
    internal sealed record CatchComponent
    {
        /// <summary>
        /// The variable that will hold the exception object.
        /// </summary>
        public required NewVariableExpression? Variable { get; init; }

        /// <summary>
        /// The list of constructs to execute if the exception is caught.
        /// e.g., statements, expressions, etc.
        /// </summary>
        public required List<Construct> Constructs { get; init; }
    }
}
