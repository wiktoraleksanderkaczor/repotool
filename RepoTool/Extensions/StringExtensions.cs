using System.Security.Cryptography;
using System.Text;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Computes the SHA256 hash of the string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The lowercase hexadecimal representation of the SHA256 hash.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
        public static string ToSha256Hash(this string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                string hashString = Convert.ToHexString(hashBytes).ToLowerInvariant();
                return hashString;
            }
        }
    }
}