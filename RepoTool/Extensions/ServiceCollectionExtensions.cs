// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoTool.Options.Common;

namespace RepoTool.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for classes implementing <see cref="IOptionModel"/> and registers them
        /// with the DI container using the standard AddOptions&lt;T&gt;().Bind(section).ValidateOnStart() pattern.
        /// Requires Microsoft.Extensions.Options.DataAnnotations package for validation to work effectively with attributes.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the required helper method cannot be found via reflection, or if a model type lacks the required static 'Section' property.</exception>
        public static IServiceCollection AddOptionModelsWithValidation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            // Ensure validation services are registered if ValidateOnStart is used.
            // This adds the necessary IValidateOptions services. Safe to call multiple times.
            services = services.AddOptions();

            // Find the generic helper method within this class ONCE.
            MethodInfo registerMethodInfo = typeof(ServiceCollectionExtensions)
                .GetMethod(nameof(RegisterAndValidateOption), BindingFlags.NonPublic | BindingFlags.Static)
                    ?? throw new InvalidOperationException("Could not find the internal RegisterAndValidateOption<T> helper method via reflection.");

            // Find types implementing IOptionModel in the calling assembly
            IEnumerable<Type> optionModelTypes = callingAssembly
                .GetTypes()
                .Where(t =>
                    t is { IsClass: true, IsAbstract: false }
                    && t.IsAssignableTo(typeof(IOptionModel)))
                .Distinct();

            foreach ( Type modelType in optionModelTypes )
            {
                try
                {
                    // Look for a public static property named "Section".
                    PropertyInfo? sectionProperty = modelType.GetProperty("Section", BindingFlags.Public | BindingFlags.Static);

                    // Get the value of the static property.
                    string sectionName = sectionProperty
                        // Pass null for static property access.
                        ?.GetValue(null) as string
                            ?? throw new InvalidOperationException($"Static property 'Section' not found on type {modelType.FullName}.");

                    // Get the specific configuration section.
                    IConfigurationSection configSection = configuration.GetSection(sectionName);
                    // Note: We proceed even if the section doesn't exist, allowing for default values.

                    // Create a generic method instance for the specific modelType.
                    MethodInfo genericRegisterMethod = registerMethodInfo.MakeGenericMethod(modelType);

                    // Invoke RegisterAndValidateOption<modelType>(services, configSection).
                    _ = genericRegisterMethod.Invoke(null, [services, configSection]);
                }
                catch ( Exception ex ) when ( ex is not InvalidOperationException ) // Catch reflection/invocation issues, but let InvalidOperationExceptions propagate
                {
                    // Wrap the original exception for clarity if needed, or simply rethrow.
                    // Since logging is removed, rethrowing is crucial for visibility.
                    throw new InvalidOperationException($"Failed to register options for type {modelType.FullName}.", ex);
                }
            }

            return services;
        }

        /// <summary>
        /// Registers a specific options type using the standard fluent API.
        /// </summary>
        /// <typeparam name="T">The options model type, must be a class implementing IOptionModel.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="section">The configuration section to bind to.</param>
        private static void RegisterAndValidateOption<T>(IServiceCollection services, IConfigurationSection section)
            // Constraint required by AddOptions<T>
            where T : class
        {
            // Use the standard, type-safe fluent API for registration, binding, and validation.
            _ = services.AddOptions<T>()
                .Bind(section)
                // Enables DataAnnotations validation on application startup.
                .ValidateOnStart();
        }
    }
}
