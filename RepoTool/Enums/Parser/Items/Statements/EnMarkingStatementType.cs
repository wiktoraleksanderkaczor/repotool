// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Items.Statements
{
    /// <summary>
    /// Defines the possible types of marking statements.
    /// </summary>
    internal enum EnMarkingStatementType
    {
        /// <summary>
        /// Represents a label.
        /// </summary>
        Label,

        // TODO: Worry about SQL-like query languages later...
        // Transaction,
        // Rollback,
        // Commit,
        // Savepoint,
    }
}
