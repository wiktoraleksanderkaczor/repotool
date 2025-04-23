// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Enums.Parser.Common
{
    /// <summary>
    /// Unified flag enum for callable modifiers
    /// </summary>
    internal enum EnCallableModifier
    {
        /// <summary>
        /// Indicates that a callable is static (class-level)
        /// </summary>
        Static,

        /// <summary>
        /// Indicates that a callable is instance (instance-level)
        /// Indicates whether the callable is a method (associated with a class/object) or a standalone callable.
        /// </summary>
        Instance,

        /// <summary>
        /// Indicates that a callable is asynchronous
        /// </summary>
        Async,

        /// <summary>
        /// Indicates that a callable is a generator callable
        /// </summary>
        Generator,

        /// <summary>
        /// Indicates that a callable is recursive
        /// </summary>
        Recursive,

        /// <summary>
        /// Indicates that a callable is pure (no side effects)
        /// </summary>
        Pure,

        /// <summary>
        /// Indicates that a callable is higher-order (accepts or returns callables)
        /// </summary>
        HigherOrder,

        /// <summary>
        /// Indicates that a callable is variadic (accepts variable number of arguments)
        /// </summary>
        Variadic,

        /// <summary>
        /// Indicates that a callable is a constructor
        /// </summary>
        Constructor,

        /// <summary>
        /// Indicates that a callable is a destructor/finalizer
        /// </summary>
        Destructor,

        /// <summary>
        /// Indicates that a callable overrides a base class callable
        /// </summary>
        Override,

        /// <summary>
        /// Indicates that a callable can be overridden by derived classes
        /// </summary>
        Virtual,

        /// <summary>
        /// Indicates that a callable is abstract and must be implemented by derived classes
        /// </summary>
        Abstract,

        /// <summary>
        /// Indicates that a callable cannot be overridden by derived classes
        /// </summary>
        Sealed,

        /// <summary>
        /// Indicates that a callable is an operator overload
        /// </summary>
        Operator,

        /// <summary>
        /// Indicates that a callable is an extension callable
        /// </summary>
        Extension,

        /// <summary>
        /// Indicates that a callable is a getter
        /// </summary>
        Getter,

        /// <summary>
        /// Indicates that a callable is a setter
        /// </summary>
        Setter,

        /// <summary>
        /// Indicates that a callable is an operator overload
        /// </summary>
        OperatorOverload,
    }
}
