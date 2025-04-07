using RepoTool.Attributes;

/// <inheritdoc />
[ToolChoice(typeof(ExpressionSelector))]
public abstract record ExpressionConstruct : Construct;
