// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Statements;
using RepoTool.Models.Parser.Items.Statements.Common;

namespace RepoTool.Models.Parser.Items.Statements
{
    /// <inheritdoc />
    public record ImportStatement : StatementConstruct
    {
        /// <summary>
        /// Path to the module being imported.
        /// Absolute path if possible, file path provided as context.
        /// System modules only need the module path.
        /// </summary>
        /// <example>
        /// <code>
        /// using System;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ModulePath": "System"
        /// }
        /// </code>
        /// </example>
        public required string ModulePath { get; init; }

        /// <summary>
        /// Path to the module being imported.
        /// Absolute path if possible, current file path provided as context.
        /// Only applicable if the module is not a system module and imports are based on paths.
        /// </summary>
        /// <example>
        /// <code>
        /// import myModule from './my-module';
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "FilePath": "./my-module"
        /// }
        /// </code>
        /// </example>
        public required string? FilePath { get; init; }

        /// <summary>
        /// List of imports or using directives by expression, represented as structured information.
        /// </summary>
        /// <example>
        /// <code>
        /// using System.Collections.Generic;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ImportedSymbols": [{
        ///         "Name": "Generic",
        ///         "Alias": "Generic"
        ///     }]
        /// }
        /// </code>
        /// </example>
        public required List<ImportedSymbolInfo>? ImportedSymbols { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this import.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// using static System.Math;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ImportModifierFlags": ["Static"] // Assuming a 'Static' flag exists or is analogous to a custom flag
        /// }
        /// </code>
        /// </example>
        public required List<EnImportModifier> ImportModifierFlags { get; init; }
    }

    /// <summary>
    /// Represents information about a symbol being imported.
    /// </summary>
    public record ImportedSymbolInfo
    {
        /// <summary>
        /// The name of the symbol being imported.
        /// </summary>
        /// <example>
        /// <code>
        /// import { myFunction } from './my-module';
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
        /// The alias of the symbol being imported, if any.
        /// </summary>
        /// <example>
        /// <code>
        /// import { myFunction as myFunc } from './my-module';
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
