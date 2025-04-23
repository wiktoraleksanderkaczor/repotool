// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Common
{
    internal enum EnTypeInfoModifier
    {
        /// <summary>
        /// Indicates that the type is nullable
        /// </summary>
        IsNullable,

        /// <summary>
        /// Indicates that the type is generic
        /// </summary>
        IsGeneric,

        /// <summary>
        /// Indicates that the type is a primitive type
        /// </summary>
        IsPrimitive,

        /// <summary>
        /// Indicates that the type is dynamically typed
        /// </summary>
        IsDynamic,

        /// <summary>
        /// Indicates that the type is pointer
        /// </summary>
        IsPointer,

        /// <summary>
        /// Indicates that the type is object
        /// Used for classes, structs, interfaces, enums, delegates, etc.
        /// </summary>
        IsObject,

        /// <summary>
        /// Indicates that the type is duck-typed
        /// </summary>
        IsDuckTyped
    }
}
