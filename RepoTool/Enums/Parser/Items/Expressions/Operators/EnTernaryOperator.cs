// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Items.Expressions.Operators
{
    /// <summary>
    /// Defines the possible ternary operators.
    /// </summary>
    internal enum EnTernaryOperator
    {
        /// <summary>
        /// Represents the conditional ternary operator (?:).
        /// </summary>
        ConditionalTernary,

        /// <summary>
        /// Represents a range with a step operator.
        /// </summary>
        RangeBetweenWithStep,

        /// <summary>
        /// Represents a slice with start, end, and step operator.
        /// </summary>
        SliceStartEndWithStep
    }
}
