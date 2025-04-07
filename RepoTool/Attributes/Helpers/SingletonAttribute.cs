using Microsoft.Extensions.DependencyInjection;

namespace RepoTool.Attributes
{
    /// <summary>
    /// An attribute to indicate that the full content of the code must be scanned to answer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ServiceLifetimeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLifetimeAttribute"/> class.
        /// </summary>
        /// <param name="serviceLifetime">The lifetime of the service.</param>
        public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }

        public ServiceLifetime ServiceLifetime { get; }
    }
}