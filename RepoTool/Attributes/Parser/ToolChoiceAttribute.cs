// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Attributes.Parser
{
    /// <summary>
    /// An attribute to indicate which tool to use for a specific selection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal sealed class ToolChoiceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolChoiceAttribute"/> class.
        /// </summary>
        /// <param name="toolChoice">The model to choose tool.</param>
        public ToolChoiceAttribute(Type toolChoice) => ToolChoice = toolChoice;

        /// <summary>
        /// Gets the model to choose tool.
        /// </summary>
        public Type ToolChoice { get; }
    }
}
