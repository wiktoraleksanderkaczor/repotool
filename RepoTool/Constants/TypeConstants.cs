// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;

namespace RepoTool.Constants
{
    internal static class TypeConstants
    {
        // TODO: Possibly add BindingFlags.FlattenHierarchy
        public const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
    }
}
