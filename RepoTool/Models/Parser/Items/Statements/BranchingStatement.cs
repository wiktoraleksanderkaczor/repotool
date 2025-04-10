using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions.Common;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    public record BranchingStatement : StatementConstruct
    {
        /// <summary>
        /// The condition of the branching statement, which is evaluated to determine which branch to execute.
        /// </summary>
        public required ExpressionConstruct Condition { get; init; }
        
        /// <summary>
        /// Ordered list of branches because order matters.
        /// </summary>
        public required List<BranchComponent> Branches { get; init; }
        
        /// <summary>
        /// The default branch that is executed if no other conditions are met.
        /// </summary>
        public required ExpressionConstruct Default { get; init; }
    }

    /// <summary>
    /// Represents a component of a branching statement, such as an if, else if, or else block.
    /// </summary>
    public record BranchComponent
    {
        /// <summary>
        /// The condition that determines whether this branch should be executed.
        /// </summary>
        public required ExpressionConstruct Condition { get; init; }

        /// <summary>
        /// The list of constructs (statements, expressions, etc.) that are executed if this branch is taken.
        /// </summary>
        public required List<Construct> Constructs { get; init; }
    }
}