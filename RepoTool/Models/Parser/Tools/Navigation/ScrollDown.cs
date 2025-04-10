using Json.Schema.Generation;


/// <summary>
/// Represents a scrolling action in the parser, indicating how many lines to scroll.
/// </summary>
public record ScrollDown
{
    /// <summary>
    /// The number of lines to scroll, must be a positive integer greater than or equal to 1.
    /// </summary>
    [Minimum(1)]
    public required int NumberOfLines { get; init; }
}