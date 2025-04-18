// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Enums.Parser.Tools;
using RepoTool.Models.Parser.Interfaces;

/// <summary>
/// Represents a selection in the parser.
/// </summary>
public record ConstructSelector : IToolSelector<EnConstructType>
{
    /// <summary>
    /// The selected tool.
    /// </summary>
    public required EnConstructType ToolSelection { get; init; }
}