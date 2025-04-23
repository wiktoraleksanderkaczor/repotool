// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Collections;
using RepoTool.Attributes.Parser;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Recursively searches the inheritance hierarchy and implemented interfaces of a type
        /// for the <see cref="ToolChoiceAttribute"/> and returns the associated tool choice type.
        /// </summary>
        /// <param name="recordType">The type to start searching from.</param>
        /// <returns>The <see cref="Type"/> specified in the <see cref="ToolChoiceAttribute"/> if found; otherwise, the original <paramref name="recordType"/>.</returns>
        public static Type GetToolSelectorType(this Type recordType)
        {
            // Check for ToolChoice attribute on the current type

            // If the attribute is found on the current type, return its ToolChoice.
            if ( recordType
                .GetCustomAttributes(typeof(ToolChoiceAttribute), false)
                .FirstOrDefault() is ToolChoiceAttribute toolChoiceAttribute )
            {
                return toolChoiceAttribute.ToolChoice;
            }

            // Check implemented interfaces
            foreach ( Type interfaceType in recordType.GetInterfaces() )
            {
                Type result = GetToolSelectorType(interfaceType);
                // If the attribute was found in the interface hierarchy, return the result
                if ( result != interfaceType )
                {
                    return result;
                }
            }

            // If the attribute is not found on the type or its interfaces, check the base type.
            // If there is no base type (reached the top of the hierarchy without finding the attribute),
            // return the original record type as a fallback.
            return recordType.BaseType is null
                ? recordType : GetToolSelectorType(recordType.BaseType);
        }

        /// <summary>
        /// Checks if the specified type is a collection type (e.g., List, Array, etc.).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is a collection type; otherwise, <c>false</c>.</returns>
        public static bool IsCollectionType(this Type type)
        {
            return type
                .GetInterfaces().Any(i =>
                    i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        /// <summary>
        /// Checks if the specified type is defined within the "RepoTool" namespace or its sub-namespaces.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type's namespace starts with "RepoTool"; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedInRepoToolNamespace(this Type type) => type.Namespace?.StartsWith("RepoTool", StringComparison.InvariantCulture) ?? false;

        /// <summary>
        /// Gets the element type of a collection type. If the type is not a collection, it throws an exception.
        /// </summary>
        /// <param name="objectType">The type to get the element type from.</param>
        /// <returns>The element type of the collection.</returns>
        /// <exception cref="NotSupportedException">Thrown if the type is not a recognized collection type.</exception>
        public static Type? GetIterableElementType(this Type objectType)
        {
            Type? elementType = null;
            if ( objectType.IsArray )
            {
                elementType = objectType.GetElementType();
            }
            // Check common generic interfaces like IList<>, ICollection<>, IEnumerable<>
            else if ( objectType.IsGenericType )
            {
                // Find IEnumerable<T> interface
                Type? iEnumerableInterface = objectType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                if ( iEnumerableInterface != null )
                {
                    elementType = iEnumerableInterface.GetGenericArguments().FirstOrDefault();
                }
            }

            // Add checks for other collection types if needed (e.g., non-generic IList requires object type)
            if ( elementType == null )
            {
                // Attempt non-generic IEnumerable as a last resort? Items would be 'object'.
                if ( typeof(IEnumerable).IsAssignableFrom(objectType) )
                {
                    // Handle non-generic collections - element type is object. This might be too permissive.
                    elementType = typeof(object);
                    // Or throw, as strongly-typed collections are preferred.
                    // throw new NotSupportedException($"Cannot determine specific element type for non-generic iterable {iterableType.Name} at path {parserContext.ItemPath}. Strongly-typed collections recommended.");
                }
            }

            return elementType;
        }

        public static Type? GetPropertyElementType(this Type objectType)
        {
            // Get the property type
            Type? propertyType = null;
            // Find if the type implements IDictionary<TKey, TValue>
            if ( objectType.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) )
            {
                // Get the generic arguments (key and value types) from the implemented IDictionary interface
                Type[] genericArguments = objectType.GetGenericArguments();
                propertyType = objectType.GetGenericArguments()[1]; // Value type
            }
            // Add checks for other dynamic types if supported
            else
            {
                // Handle cases where the object type isn't a recognized dynamic type like Dictionary

            }

            return propertyType;
        }
    }
}
