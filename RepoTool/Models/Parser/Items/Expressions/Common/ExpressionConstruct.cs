// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Items.Common;
using RepoTool.Models.Parser.Tools.Selectors;

namespace RepoTool.Models.Parser.Items.Expressions.Common
{
    /// <inheritdoc />
    [ToolChoice(typeof(ExpressionSelector))]
    public abstract record ExpressionConstruct : Construct;
}
