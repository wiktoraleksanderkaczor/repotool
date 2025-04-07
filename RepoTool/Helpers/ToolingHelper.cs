namespace RepoTool.Helpers
{
    /// <summary>
    /// Helper class for tool-related operations.
    /// </summary>
    public static class ToolingHelper
    {
        /// <summary>
        /// Gets the tool prompt from the XML comment blocks.
        /// e.g. &lt;summary&gt;, &lt;remarks&gt;, &lt;example&gt; which contains &lt;code&gt; tags. 
        /// </summary>
        /// <param name="type">The type to get the tool prompt from.</param>
        /// <returns>The tool prompt as a string.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static string GetToolPrompt(Type type)
        {
            return "Nothing";
        }
    }
}