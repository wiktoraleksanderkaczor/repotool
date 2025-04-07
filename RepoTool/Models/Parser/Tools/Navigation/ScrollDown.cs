using Json.Schema.Generation;


/// <summary>
/// Represents a scrolling action in the parser, indicating how many lines to scroll.
/// </summary>
public record ScrollDown
{
    /// <summary>
    /// The number of lines to scroll, must be a positive integer.
    /// </summary>
    [Minimum(1)]
    public required int NumberOfLines { get; init; }
}