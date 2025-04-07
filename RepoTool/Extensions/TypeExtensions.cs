using Json.Schema.Generation;
using RepoTool.Attributes;

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
            ToolChoiceAttribute? toolChoiceAttribute = recordType
                .GetCustomAttributes(typeof(ToolChoiceAttribute), false)
                .FirstOrDefault() as ToolChoiceAttribute;

            // If the attribute is found on the current type, return its ToolChoice.
            if (toolChoiceAttribute != null)
            {
                return toolChoiceAttribute.ToolChoice;
            }

            // Check implemented interfaces
            foreach (Type interfaceType in recordType.GetInterfaces())
            {
                Type result = GetToolSelectorType(interfaceType);
                // If the attribute was found in the interface hierarchy, return the result
                if (result != interfaceType)
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
        /// Determines whether the specified type or any of its generic arguments (recursively)
        /// are marked with the <see cref="JsonExcludeAttribute"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <c>true</c> if the type, its generic definition, or any of its nested generic types have the
        /// <see cref="JsonExcludeAttribute"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsJsonExclude(this Type type)
        {
            // Check if the current type itself has the [JsonExclude] attribute
            if (type.GetCustomAttributes(typeof(JsonExcludeAttribute), false).Length != 0)
            {
                return true;
            }

            // If the type is generic, check its definition and recursively check its arguments
            if (type.IsGenericType)
            {
                // Check the generic type definition (e.g., List<> in List<string>)
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition.GetCustomAttributes(typeof(JsonExcludeAttribute), false).Length != 0)
                {
                    return true;
                }

                // Recursively check each generic argument
                foreach (Type genericArgument in type.GetGenericArguments())
                {
                    if (ContainsJsonExclude(genericArgument))
                    {
                        return true;
                    }
                }
            }

            // If no [JsonExclude] attribute was found on the type or its nested generic types
            return false;
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
        public static bool IsDefinedInRepoToolNamespace(this Type type)
        {
            return type.Namespace?.StartsWith("RepoTool") ?? false;
        }
    }
}