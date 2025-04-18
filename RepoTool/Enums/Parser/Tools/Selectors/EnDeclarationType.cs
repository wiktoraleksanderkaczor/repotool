// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using RepoTool.Attributes.Parser;
using RepoTool.Models.Parser.Items.Declarations;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Enums.Parser.Tools.Selectors
{
    /// <summary>
    /// Defines the possible types of declarations.
    /// </summary>
    public enum EnDeclarationType
    {
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a delegate declaration.
        /// </summary>
        [ItemChoice(typeof(DelegateDeclaration))]
        Delegate,

        /// <summary>
        /// Represents a property declaration.
        /// </summary>
        [ItemChoice(typeof(PropertyDeclaration))]
        Property,

        /// <summary>
        /// Represents a field declaration.
        /// </summary>
        [ItemChoice(typeof(FieldDeclaration))]
        Field,

        /// <summary>
        /// Represents an event declaration.
        /// </summary>
        [ItemChoice(typeof(EventDeclaration))]
        Event
    }
}
