// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Security.Cryptography;
using System.Text;
using Spectre.Console;
using Spectre.Console.Json;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string"/> class.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Computes the SHA256 hash of the string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The lowercase hexadecimal representation of the SHA256 hash.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
        public static string ToSha256Hash(this string input)
        {
            ArgumentNullException.ThrowIfNull(input);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = SHA256.HashData(inputBytes);
            string hashString = Convert.ToHexString(hashBytes).ToUpperInvariant();
            return hashString;
        }

        public static void DisplayAsJson(this string json, Color color)
        {
            try
            {
                JsonText jsonText = new(json);
                AnsiConsole.Write(
                    new Panel(jsonText)
                        .Header("Action")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(color));
            }
            catch ( InvalidOperationException )
            {
                AnsiConsole.WriteLine("The output is not a valid JSON.");
                AnsiConsole.Write(
                    new Panel(json)
                        .Header("Action")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(color));
            }
        }
    }
}
