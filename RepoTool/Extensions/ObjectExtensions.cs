using RepoTool.Helpers;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts the <see cref="object"/> to a JSON string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToJson(this object obj)
        {
            return JsonHelper.SerializeToJson(obj);
        }
    }
}