// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Tools.Common
{
    public enum EnInheritanceModifier
    {
        /// <summary>
        /// Indicates that the inheritance is abstract and cannot be instantiated
        /// </summary>
        Abstract,

        /// <summary>
        /// Indicates that a item is static and cannot be instantiated
        /// </summary>
        Static,

        /// <summary>
        /// Indicates that a item cannot be inherited from
        /// </summary>
        Sealed,

        /// <summary>
        /// Indicates that a item is virtual and can be overriden
        /// </summary>
        Virtual,

        /// <summary>
        /// Indicates that a item overrides a base item property
        /// </summary>
        Override,

        /// <summary>
        /// Indicates that the item is defined in another place
        /// </summary>
        Extern,

        /// <summary>
        /// Indicates that a item is new and hides a base item property
        /// </summary>
        New,
    }
}
