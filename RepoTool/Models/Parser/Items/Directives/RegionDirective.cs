using Json.Schema.Generation;


/// <summary>
/// Represents a region preprocessor directive (also covers endregion).
/// </summary>
/// <inheritdoc />
public record RegionDirective : DirectiveConstruct
{
    /// <summary>
    /// The name/description of the region.
    /// </summary>
    /// <example>
    /// <code>
    /// #region MyRegion
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///   "Name": "MyRegion"
    /// }
    /// </code>
    /// </example>
    public string? Name { get; init; }
    
    /// <summary>
    /// The constructs contained within this region.
    /// </summary>
    [JsonExclude]
    public required List<Construct> Constructs { get; init; }
    
    /// <summary>
    /// Describes the purpose of this region in the code.
    /// </summary>
     /// <example>
    /// <code>
    /// #region MyRegion // This region contains utility functions.
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///    "RegionPurpose": "This region contains utility functions."
    /// }
    /// </code>
    /// </example>
    public required string RegionPurpose { get; init; }
}