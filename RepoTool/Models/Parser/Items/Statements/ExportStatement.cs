using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    public record ExportStatement : StatementConstruct
    {
        /// <summary>
        /// Path to the module being imported.
        /// Absolute path if possible, file path provided as context.
        /// System modules only need the module path.
        /// </summary>
        /// <example>
        /// <code>
        /// export { myFunction } from './my-module';
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "ModulePath": "./my-module"
        /// }
        /// </code>
        /// </example>
        public required string ModulePath { get; init; }

        /// <summary>
        /// List of imports or using directives by expression, represented as structured information.
        /// </summary>
        /// <example>
        /// <code>
        /// export { myFunction } from './my-module';
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ImportedSymbols": [{
        ///         "Name": "myFunction",
        ///         "Alias": "myFunction"
        ///     }]
        /// }
        /// </code>
        /// </example>
        public required List<ExportedSymbolInfo>? ImportedSymbols { get; init; }
    }

    /// <summary>
    /// Represents information about a symbol being exported.
    /// </summary>
    public record ExportedSymbolInfo
    {
        /// <summary>
        /// The name of the symbol being exported.
        /// </summary>
        /// <example>
        /// <code>
        /// export { myFunction } from './my-module';
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Name": "myFunction"
        /// }
        /// </code>
        /// </example>
        public required string Name { get; init; }

        /// <summary>
        /// The alias of the symbol being exported, if any.
        /// </summary>
        /// <example>
        /// <code>
        /// export { myFunction as myFunc } from './my-module';
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Alias": "myFunc"
        /// }
        /// </code>
        /// </example>
        public required string Alias { get; init; }
    }
}