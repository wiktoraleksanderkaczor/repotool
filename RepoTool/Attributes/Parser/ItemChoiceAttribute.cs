// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Attributes.Parser
{
    /// <summary>
    /// An attribute to indicate which item to instantiate for a specific choice in a selection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ItemChoiceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemChoiceAttribute"/> class.
        /// </summary>
        /// <param name="itemChoice">The model to choose item.</param>
        public ItemChoiceAttribute(Type itemChoice) => ItemChoice = itemChoice;

        /// <summary>
        /// Gets the model to choose item.
        /// </summary>
        public Type ItemChoice { get; }
    }
}
