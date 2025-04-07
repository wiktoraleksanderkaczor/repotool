using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RepoTool.Persistence;
using RepoTool.Constants;
using Microsoft.Extensions.Hosting;
using RepoTool.Extensions;
using System.Reflection;
using RepoTool.Attributes;

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

        private static void ConfigureOptions(HostBuilderContext context, IServiceCollection services)
        {
            // Add options for the configuration implementing IOptionModel
            services.AddOptionModelsWithValidation(context.Configuration);
        }

        private static void ConfigureDatabase(IServiceCollection services)
        {
            string connectionString = $"Data Source={PathConstants.DatabasePath}";
            services.AddDbContext<RepoToolDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        private static void ConfigureHelpers(IServiceCollection services)
        {
            List<Type> helpers = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(type =>
                    !type.IsAbstract
                    && !type.IsInterface
                    && type.Name.EndsWith("Helper"))
                .ToList();

            foreach (Type type in helpers)
            {
                ServiceLifetimeAttribute? serviceLifetimeAttribute = type
                    .GetCustomAttribute(typeof(ServiceLifetimeAttribute))
                    as ServiceLifetimeAttribute;

                // Default to Transient if no attribute is found
                ServiceLifetime lifetime = serviceLifetimeAttribute?.ServiceLifetime ?? ServiceLifetime.Transient;

                // Register the helper type itself
                switch(lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type);
                        break;
                }

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

        private static void ConfigureJsonSchemaVocabularies()
        {
            Json.Schema.OpenApi.Vocabularies.Register();
        }
    }   
}