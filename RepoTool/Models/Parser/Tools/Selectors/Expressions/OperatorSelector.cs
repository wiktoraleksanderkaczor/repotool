/// <summary>
/// Defines the possible types of operators.
/// </summary>
public enum EnOperatorType
{
    /// <summary>
    /// Represents a unary operator (e.g., -, !).
    /// </summary>
    Unary,
    /// <summary>
    /// Represents a binary operator (e.g., +, -, *, /).
    /// </summary>
    Binary,
    /// <summary>
    /// Represents a ternary operator (e.g., the conditional operator ?:)
    /// </summary>
    Ternary,
}

public record OperatorSelector
{
    /// <summary>
    /// The type of the operator.
    /// </summary>
    /// <example>
    /// <code>
    /// a + b
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "OperatorType": "Binary"
    /// }
    /// </code>
    /// </example>
    public required EnOperatorType OperatorType { get; init; }
}