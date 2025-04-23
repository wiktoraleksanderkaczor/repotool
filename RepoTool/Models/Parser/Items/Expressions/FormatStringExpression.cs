// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Expressions;
using RepoTool.Models.Parser.Items.Expressions.Common;

namespace RepoTool.Models.Parser.Items.Expressions
{
    /// <inheritdoc />
    internal sealed record FormatStringExpression : ExpressionConstruct
    {
        /// <summary>
        /// The components of the format string.
        /// </summary>
        /// <example>
        /// <code>
        /// $"Hello, {name}!"
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Components": [
        ///         { "ComponentType": "Text", "Text": "Hello, " },
        ///         { "ComponentType": "Expression", /* ... Expression details ... */ },
        ///         { "ComponentType": "Text", "Text": "!" }
        ///     ]
        /// }
        /// </code>
        /// </example>
        public required List<FormatStringComponent> Components { get; init; }
    }

    /// <summary>
    /// Represents a component of a format string.
    /// </summary>
    internal record FormatStringComponent
    {
        /// <summary>
        /// The type of the format string component.
        /// </summary>
        /// <example>
        /// <code>
        /// "Hello, "
        /// </code>
        /// Would have a component parsed as:
        /// <code>
        /// {
        ///    "ComponentType": "Text"
        /// }
        /// </code>
        /// On the other hand, an expression like:
        /// <code>
        /// {value:F2}
        /// </code>
        /// Would have a component parsed as:
        /// <code>
        /// {
        ///    "ComponentType": "Expression"
        /// }
        /// </code>
        /// </example>
        public required EnFormatStringComponentType ComponentType { get; init; }
    }

    /// <inheritdoc />
    internal sealed record FormatStringTextComponent : FormatStringComponent
    {
        /// <summary>
        /// The text content of the component.
        /// </summary>
        /// <example>
        /// <code>
        /// $"Hello, {name}!"
        /// </code>
        /// Would have a text component parsed as:
        /// <code>
        /// {
        ///     "Text": "Hello, "
        /// }
        /// </code>
        /// </example>
        public required string? Text { get; init; }
    }

    /// <inheritdoc />
    internal sealed record FormatStringExpressionComponent : FormatStringComponent
    {
        /// <summary>
        /// The expression contained within the format string.
        /// </summary>
        public required ExpressionConstruct? Expression { get; init; }

        /// <summary>
        /// The pipes applied to the expression in the format string.
        /// </summary>
        /// <example>
        /// <code>
        /// $"{value:F2}"
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "Pipes": [{ "Purpose": "Format as fixed-point with 2 decimal places" }]
        /// }
        /// </code>
        /// </example>
        public required List<FormatStringExpressionPipe>? Pipes { get; init; }
    }

    /// <summary>
    /// Represents a pipe applied to an expression within a format string.
    /// </summary>
    internal sealed record FormatStringExpressionPipe
    {
        /// <summary>
        /// Specifies what the pipe is meant to do.
        /// e.g. Show the value to three decimal places
        /// e.g. Show the value in HH:mm:ss format
        /// </summary>
        /// <example>
        /// <code>
        /// {value:F2}
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "Purpose": "Format as fixed-point with 2 decimal places"
        /// }
        /// </code>
        /// </example>
        public required string Purpose { get; init; }
    }
}
