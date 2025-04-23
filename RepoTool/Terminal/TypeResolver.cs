// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace RepoTool.Terminal
{
    internal sealed class TypeResolver : ITypeResolver
    {
        private readonly IServiceProvider _provider;

        public TypeResolver(IServiceProvider provider) => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

        public object? Resolve(Type? type)
        {
            return type != null
                ? _provider.GetRequiredService(type) : null;
        }
    }
}
