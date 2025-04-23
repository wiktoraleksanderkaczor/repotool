// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace RepoTool.Terminal
{
    internal sealed class TypeRegistrar : ITypeRegistrar
    {
        private IServiceCollection _builder;

        public TypeRegistrar(IServiceCollection builder) => _builder = builder;

        public ITypeResolver Build() => new TypeResolver(_builder.BuildServiceProvider());

        public void Register(Type service, Type implementation) => _builder.AddSingleton(service, implementation);

        public void RegisterInstance(Type service, object implementation) => _builder.AddSingleton(service, implementation);

        public void RegisterLazy(Type service, Func<object> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            _builder = _builder.AddSingleton(service, (_) => factory());
        }
    }
}
