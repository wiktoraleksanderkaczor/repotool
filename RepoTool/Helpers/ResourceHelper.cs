// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using RepoTool.Constants;

namespace RepoTool.Helpers
{
    public static class ResourceHelper
    {
        public static string GetParserLanguagesJson()
        {
            return GetResourceContent(ResourceConstants.ParserLanguages)
                ?? throw new FileNotFoundException("Parser languages JSON file not found.");
        }

        public static string GetModelDocumentation()
        {
            return GetResourceContent(ResourceConstants.ModelDocumentation)
                ?? throw new FileNotFoundException("Model documentation file not found.");
        }

        /// <summary>
        /// Gets the content of an embedded resource file.
        /// </summary>
        /// <param name="path">The path of the resource file.</param>
        /// <returns>The content of the resource file, or null if not found.</returns>
        public static string? GetResourceContent(string path)
        {
            string filePath = Path.Combine(PathConstants.UserRepoToolFolder, path);
            if ( File.Exists(filePath) )
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string? resourceName = assembly.GetManifestResourceNames()
                    .FirstOrDefault(name => name.Contains(path));

                if ( resourceName == null )
                {
                    return null;
                }
                else
                {
                    using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
                    using StreamReader reader = new(stream);
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Get all template resource names
        /// </summary>
        /// <returns>List of template embedded resource names</returns>
        public static List<string> GetTemplateResourceNames()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            List<string> resourceNames = assembly
                .GetManifestResourceNames()
                .Where(r => r.EndsWith(".sbn", StringComparison.InvariantCulture))
                .ToList();

            return resourceNames;
        }
    }
}
