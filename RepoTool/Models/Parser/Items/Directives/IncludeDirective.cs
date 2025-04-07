/// <summary>
/// Represents an include preprocessor directive.
/// </summary>
/// <inheritdoc />
public record IncludeDirective : DirectiveConstruct
{
    /// <summary>
    /// The path of the file to include.
    /// </summary>
    /// <example>
    /// <code>
    /// #include "myfile.h"
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///   "FilePath": "myfile.h"
    /// }
    /// </code>
    /// </example>
    public required string FilePath { get; init; }
    
    /// <summary>
    /// The purpose of including this file.
    /// </summary>
    /// <example>
    /// <code>
    /// #include "myfile.h" // Included for utility functions
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///    "InclusionPurpose": "Included for utility functions"
    /// }
    /// </code>
    /// </example>
    public required string InclusionPurpose { get; init; }
}