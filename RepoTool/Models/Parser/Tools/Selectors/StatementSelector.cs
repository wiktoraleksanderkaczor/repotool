using RepoTool.Attributes;

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

public record StatementSelector : IToolSelector<EnStatementType>
{
    /// <summary>
    /// The type of the statement.
    /// </summary>
    /// <example>
    /// <code>
    /// return 0;
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "ToolSelection": "ControlFlow"
    /// }
    /// </code>
    /// </example>
    public required EnStatementType ToolSelection { get; init; }
}