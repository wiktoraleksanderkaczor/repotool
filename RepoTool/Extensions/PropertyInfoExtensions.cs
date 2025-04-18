// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using Json.Schema.Generation;
using RepoTool.Attributes.Parser;
using RepoTool.Flags.Parser;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="PropertyInfo"/>.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Gets the <see cref="JsonSpecialFlag"/> for a given property if specific attributes are present.
        /// This method checks for the presence of <see cref="FullContentScanAttribute"/> or <see cref="UniqueItemsAttribute"/>
        /// and sets the corresponding flags. Returns null if neither attribute is found.
        /// </summary>
        /// <param name="propertyInfo">The property information to inspect.</param>
        /// <returns>A <see cref="JsonSpecialFlag"/> value if relevant attributes are found; otherwise, null.</returns>
        public static JsonSpecialFlag GetJsonSpecialFlag(this PropertyInfo propertyInfo)
        {
            JsonSpecialFlag flag = JsonSpecialFlag.None;

            FullContentScanAttribute? fullContentScanAttribute = propertyInfo.GetCustomAttribute<FullContentScanAttribute>();
            if ( fullContentScanAttribute != null )
            {
                flag |= JsonSpecialFlag.FullContentScan;
            }

            UniqueItemsAttribute? uniqueItemsAttribute = propertyInfo.GetCustomAttribute<UniqueItemsAttribute>();
            if ( uniqueItemsAttribute != null )
            {
                flag |= JsonSpecialFlag.UniqueItems;
            }

            // Return the flag only if at least one relevant attribute was found
            return flag;
        }
    }
}
