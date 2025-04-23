// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Tools.Selectors.Expressions
{
    /// <summary>
    /// Defines the possible types of operators.
    /// </summary>
    internal enum EnOperatorType
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
}
