// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Extensions
{
    internal static class EnumExtensions
    {
        public static bool HasMultipleFlags<T>(this T value) where T : struct, Enum
        {
            Type type = typeof(T);

            // Get all enum values
            Array enumValues = Enum.GetValues(type);
            int matchCount = 0;

            long longValue = Convert.ToInt64(value, null);

            foreach ( Enum enumVal in enumValues )
            {
                long enumLong = Convert.ToInt64(enumVal, null);
                if ( enumLong != 0 && ( longValue & enumLong ) == enumLong )
                {
                    matchCount++;
                    if ( matchCount > 1 )
                    {
                        return true; // Multiple flags detected
                    }
                }
            }

            return false; // Zero or only one flag
        }

        /// <summary>
        /// Gets a list of individual flags that are set in an enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>A list of set flags.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="ArgumentException">Thrown if T is not an enum type.</exception>
        public static List<T> GetFlags<T>(this T value) where T : struct, Enum
        {
            Type type = typeof(T);

            // Ensure T is an enum type
            if ( !type.IsEnum )
            {
                throw new ArgumentException("Value must be an enum type.", nameof(value));
            }

            List<T> flags = [];
            long longValue = Convert.ToInt64(value, null);

            // Iterate through all defined enum values
            foreach ( T enumVal in Enum.GetValues(type).Cast<T>() )
            {
                long enumLong = Convert.ToInt64(enumVal, null);

                // Check if the enum value is a non-zero flag and is set in the input value
                if ( enumLong != 0 && ( longValue & enumLong ) == enumLong )
                {
                    flags.Add(enumVal);
                }
            }

            // Handle the case where the input value itself is zero (often representing 'None')
            // If the input was 0 and 0 is a defined enum value, return it.
            if ( longValue == 0 && Enum.IsDefined(type, value) )
            {
                flags.Add(value);
            }

            return flags;
        }
    }
}
