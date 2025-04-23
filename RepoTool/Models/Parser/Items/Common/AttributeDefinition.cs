// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Items.Common;
using RepoTool.Models.Parser.Items.Expressions;

namespace RepoTool.Models.Parser.Items.Common
{
    /// <summary>
    /// Represents the definition of an attribute.
    /// </summary>
    internal sealed record AttributeDefinition : Construct
    {
        /// <summary>
        /// The target of the attribute.
        /// </summary>
        public required EnAttributeTarget AttributeTarget { get; init; }

        /// <summary>
        /// The attribute usage expression.
        /// </summary>
        public required CallableUseExpression Attribute { get; init; }
    }
}
