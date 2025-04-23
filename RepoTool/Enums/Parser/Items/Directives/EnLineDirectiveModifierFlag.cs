// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Items.Directives
{
    /// <summary>
    /// Represents flags for line preprocessor directive modifiers (hidden, default).
    /// </summary>
    internal enum EnLineDirectiveModifier
    {
        /// <summary>
        /// Represents setting a specific line number.
        /// </summary>
        SetNumber,

        /// <summary>
        /// Represents the #line hidden directive.
        /// </summary>
        Hidden,

        /// <summary>
        /// Represents the #line default directive.
        /// </summary>
        Default,
    }
}
