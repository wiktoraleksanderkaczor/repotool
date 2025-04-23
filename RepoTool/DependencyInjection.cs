// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepoTool.Attributes.Helpers;
using RepoTool.Constants;
using RepoTool.Extensions;
using RepoTool.Persistence;

namespace RepoTool
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            ConfigureOptions(context, services);
            ConfigureDatabase(services);
            ConfigureHelpers(services);
            ConfigureJsonSchemaVocabularies();
        }

        private static void ConfigureOptions(HostBuilderContext context, IServiceCollection services) =>
            // Add options for the configuration implementing IOptionModel
            services.AddOptionModelsWithValidation(context.Configuration);

        private static void ConfigureDatabase(IServiceCollection services)
        {
            string connectionString = $"Data Source={PathConstants.DatabasePath}";
            services = services.AddDbContext<RepoToolDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        private static void ConfigureHelpers(IServiceCollection services)
        {
            List<Type> helpers = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(type =>
                    !type.IsAbstract
                    && !type.IsInterface
                    && type.Name.EndsWith("Helper", StringComparison.InvariantCulture))
                .ToList();

            foreach ( Type type in helpers )
            {
                ServiceLifetimeAttribute? serviceLifetimeAttribute = type
                    .GetCustomAttribute<ServiceLifetimeAttribute>();

                // Default to Transient if no attribute is found
                ServiceLifetime lifetime = serviceLifetimeAttribute?.ServiceLifetime ?? ServiceLifetime.Transient;

                // Register the helper type itself
                services = lifetime switch
                {
                    ServiceLifetime.Singleton => services.AddSingleton(type),
                    ServiceLifetime.Scoped => services.AddScoped(type),
                    ServiceLifetime.Transient => services.AddTransient(type),
                    _ => throw new InvalidOperationException($"Invalid service lifetime '{lifetime}' specified for type '{type.FullName}'."),
                };

                // TODO: Register the Lazy<T> version of the helper
                // Type lazyType = typeof(Lazy<>).MakeGenericType(type);
                // switch(lifetime)
                // {
                //     case ServiceLifetime.Singleton:
                //         services.AddSingleton(lazyType, provider => provider.GetRequiredService(type));
                //         break;
                //     case ServiceLifetime.Scoped:
                //         services.AddScoped(lazyType, provider => provider.GetRequiredService(type));
                //         break;
                //     case ServiceLifetime.Transient:
                //         // Registering Lazy<T> as Transient means a new Lazy wrapper is created each time it's requested.
                //         // The underlying service T will be resolved according to its own registration lifetime (done above)
                //         // when the Lazy<T>.Value is accessed for the first time *by that specific Lazy instance*.
                //         services.AddTransient(lazyType, provider => provider.GetRequiredService(type));
                //         break;
                // }
            }
        }

        private static void ConfigureJsonSchemaVocabularies() => Json.Schema.OpenApi.Vocabularies.Register();
    }
}
