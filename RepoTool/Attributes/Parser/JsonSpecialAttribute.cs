// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Attributes.Parser
{
    /// <summary>
    /// An attribute to indicate that the full content of the code must be scanned to answer.
    /// Only applicable to iterable properties, it will be ignored for other types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FullContentScanAttribute : Attribute;
}
