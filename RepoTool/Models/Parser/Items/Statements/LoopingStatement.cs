// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    internal sealed record LoopingStatement : StatementConstruct
    {
        /// <summary>
        /// Expression that is evaluated before the loop starts. e.g. int i = 0;
        /// </summary>
        public required ExpressionConstruct? Initial { get; init; }

        /// <summary>
        /// Expression that is evaluated before each iteration of the loop. e.g. i &lt; 10;
        /// </summary>
        public required ExpressionConstruct Condition { get; init; }

        /// <summary>
        /// Expression that is evaluated after each iteration of the loop. e.g. i++;
        /// </summary>
        public required ExpressionConstruct? Increment { get; init; }

        /// <summary>
        /// Ordered list of statements that are executed in the loop.
        /// </summary>
        public required List<Construct> Constructs { get; init; }

        /// <summary>
        /// Whether the loop is at least once. e.g. for(;;) or while(true) or do-while()
        /// </summary>
        /// <example>
        /// <code>
        /// do { ... } while(true);
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "IsAtLeastOnce": true
        /// }
        /// </code>
        /// </example>
        public bool IsAtLeastOnce { get; init; }
    }
}
