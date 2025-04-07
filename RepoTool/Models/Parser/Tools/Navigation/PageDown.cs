using Json.Schema.Generation;


/// <summary>
/// Represents a page down action in the parser, indicating how many lines to scroll.
/// </summary>
public record PageDown
{
    /// <summary>
    /// The number of overlapping lines for next page to current page.
    /// This is used to determine how many lines to scroll down.
    /// Must be a positive integer.
    /// </summary>
    [Minimum(1)]
    public required int OverlappingLines { get; init; }
}