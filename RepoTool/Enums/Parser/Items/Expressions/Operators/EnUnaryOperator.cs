// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Items.Expressions.Operators
{
    /// <summary>
    /// Defines the possible unary operators.
    /// </summary>
    internal enum EnUnaryOperator
    {
        /// <summary>
        /// Represents the logical NOT operator (!).
        /// </summary>
        LogicalNot,

        /// <summary>
        /// Represents the bitwise NOT operator (~).
        /// </summary>
        BitwiseNot,

        /// <summary>
        /// Represents the increment operator (++).
        /// </summary>
        Increment,

        /// <summary>
        /// Represents the decrement operator (--).
        /// </summary>
        Decrement,

        /// <summary>
        /// Represents the unary plus operator (+).
        /// </summary>
        UnaryPlus,

        /// <summary>
        /// Represents the unary minus operator (-).
        /// </summary>
        UnaryMinus,

        /// <summary>
        /// Address-of operator (&amp;) in C++.
        /// </summary>
        AddressOf,

        /// <summary>
        /// Dereference operator (*) in C++.
        /// </summary>
        Dereference,

        /// <summary>
        /// Python-style unpacking operator (*).
        /// </summary>
        Unpacking
    }
}
