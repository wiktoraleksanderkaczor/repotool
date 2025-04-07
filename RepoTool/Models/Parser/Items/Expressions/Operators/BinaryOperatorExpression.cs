using Json.Schema.Generation;

/// <summary>
/// Assignment operators to be covered by translating to full assignment statements.
/// </summary>
/// <summary>
/// Assignment operators to be covered by translating to full assignment statements.
/// </summary>
public enum EnBinaryOperator
{
    /// <summary>
    /// Represents the addition operator (+).
    /// </summary>
    Add,

    /// <summary>
    /// Represents the subtraction operator (-).
    /// </summary>
    Subtract,

    /// <summary>
    /// Represents the multiplication operator (*).
    /// </summary>
    Multiply,

    /// <summary>
    /// Represents the division operator (/).
    /// </summary>
    Divide,

    /// <summary>
    /// Represents the modulo operator (%).
    /// </summary>
    Modulo,

    /// <summary>
    /// Represents the power operator (**).
    /// </summary>
    Power,

    /// <summary>
    /// Represents the equality operator (==).
    /// </summary>
    Equal,

    /// <summary>
    /// Represents the inequality operator (!=).
    /// </summary>
    NotEqual,

    /// <summary>
    /// Represents the greater than operator (>).
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Represents the less than operator (&lt;).
    /// </summary>
    LessThan,

    /// <summary>
    /// Represents the greater than or equal to operator (>=).
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Represents the less than or equal to operator (&lt;=).
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Represents the logical AND operator (&amp;&amp;).
    /// </summary>
    LogicalAnd,

    /// <summary>
    /// Represents the logical OR operator (||).
    /// </summary>
    LogicalOr,

    /// <summary>
    /// Represents the bitwise AND operator (&amp;).
    /// </summary>
    BitwiseAnd,

    /// <summary>
    /// Represents the bitwise OR operator (|).
    /// </summary>
    BitwiseOr,

    /// <summary>
    /// Represents the bitwise XOR operator (^).
    /// </summary>
    BitwiseXor,

    /// <summary>
    /// Represents the bitwise left shift operator (&lt;&lt;).
    /// </summary>
    BitwiseLeftShift,

    /// <summary>
    /// Represents the bitwise right shift operator (>>).
    /// </summary>
    BitwiseRightShift,

    /// <summary>
    /// Represents the null coalescing operator (??).
    /// </summary>
    NullCoalescing,

     /// <summary>
    /// Represents reference equality (===).
    /// </summary>
    ReferenceEqual,

     /// <summary>
    /// Represents reference inequality (!==).
    /// </summary>
    ReferenceNotEqual,

    /// <summary>
    /// Represents pointer to member operator (->).
    /// </summary>
    PointerToMember,

    /// <summary>
    /// Represents the range between operator (..).
    /// </summary>
    RangeBetween,

    /// <summary>
    /// Represents the slice with start and end (a:b).
    /// </summary>
    SliceStartEnd
}

/// <inheritdoc />
public record BinaryOperatorExpression : OperatorExpression
{
    /// <summary>
    /// The binary operator.
    /// </summary>
    /// <example>
    /// <code>
    /// a + b
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "Operator": "Add"
    /// }
    /// </code>
    /// </example>
    public required EnBinaryOperator Operator { get; init; }

    /// <summary>
    /// The left operand of the binary operator.
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct? Left { get; init; }
    
    /// <summary>
    /// The right operand of the binary operator.
    /// </summary>
    [JsonExclude]
    public required ExpressionConstruct? Right { get; init; }
}