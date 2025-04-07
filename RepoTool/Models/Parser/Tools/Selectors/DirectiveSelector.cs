using RepoTool.Attributes;

/// <summary>
/// Defines the possible types of preprocessor directives.
/// </summary>
public enum EnDirectiveType
{
    /// <summary>
    /// Represents a scroll down action.
    /// </summary>
    [ToolChoice(typeof(ScrollDown))]
    ScrollDown,

    /// <summary>
    /// Represents a custom directive.
    /// </summary>
    [ItemChoice(typeof(CustomDirective))]
    Custom,

    /// <summary>
    /// Represents a diagnostic directive (covers both Warning and Error).
    /// </summary>
    [ItemChoice(typeof(DiagnosticDirective))]
    Diagnostic, // Covers Warning and Error

    /// <summary>
    /// Represents a line directive (covers hidden, default, and value).
    /// </summary>
    [ItemChoice(typeof(LineDirective))]
    Line, // covers hidden, default and value

    /// <summary>
    /// Represents a region directive (also covers endregion).
    /// </summary>
    [ItemChoice(typeof(RegionDirective))]
    Region, // Also covers endregion

    /// <summary>
    /// Represents an include directive.
    /// </summary>
    [ItemChoice(typeof(IncludeDirective))]
    Include,

    /// <summary>
    /// Represents a branching directive.
    /// </summary>
    [ItemChoice(typeof(BranchingDirective))]
    Branching,
    
    /// <summary>
    /// Represents a definition directive (for defining and undefining symbols).
    /// </summary>
    [ItemChoice(typeof(DefinitionDirective))]
    Definition, // with enum types for definiting and undefinition

    /// <summary>
    /// Stores the full command as a List&lt;string&gt; because they are compiler-dependent.
    /// </summary>
    [ItemChoice(typeof(PragmaDirective))]
    Pragma
}

public record DirectiveSelector : IToolSelector<EnDirectiveType>
{
    /// <summary>
    /// The type of the preprocessor directive.
    /// </summary>
    /// <example>
    /// <code>
    /// #region MyRegion
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "ToolSelection": "Region"
    /// }
    /// </code>
    /// </example>
    public required EnDirectiveType ToolSelection { get; init; }
}