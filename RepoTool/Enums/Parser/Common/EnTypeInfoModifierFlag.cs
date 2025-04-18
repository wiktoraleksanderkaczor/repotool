// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Common
{
    public enum EnTypeInfoModifierFlag
    {
        /// <summary>
        /// Indicates that the type is nullable
        /// </summary>
        Nullable,

        /// <summary>
        /// Indicates that the type is generic
        /// </summary>
        Generic,

        /// <summary>
        /// Indicates that the type is a primitive type
        /// </summary>
        Primitive,

        /// <summary>
        /// Indicates that the type is dynamically typed
        /// </summary>
        Dynamic,

        /// <summary>
        /// Indicates that the type is pointer
        /// </summary>
        Pointer,

        /// <summary>
        /// Indicates that the type is object
        /// Used for classes, structs, interfaces, enums, delegates, etc.
        /// </summary>
        Object,

        /// <summary>
        /// Indicates that the type is duck-typed
        /// </summary>
        DuckTyped
    }
}
