// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

namespace RepoTool.Constants
{
    /// <summary>
    /// Defines constant values for file system paths used throughout the application.
    /// </summary>
    internal static class PathConstants
    {
        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>
        public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        /// <summary>
        /// Gets the path to the user-specific .repotool folder in the user's profile directory.
        /// </summary>
        public static readonly string UserRepoToolFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".repotool");

        /// <summary>
        /// Gets the path to the schemas subfolder within the user-specific .repotool folder.
        /// </summary>
        public static readonly string UserRepoToolSchemaFolder = Path.Combine(
            UserRepoToolFolder, "schemas"); // Removed redundant ".repotool" segment

        /// <summary>
        /// Gets the path to the .repotool folder within the current working directory.
        /// </summary>
        public static readonly string RepoToolFolder = Path.Combine(
            CurrentDirectory, ".repotool");

        /// <summary>
        /// Gets the path to the .git folder within the current working directory.
        /// </summary>
        public static readonly string GitFolder = Path.Combine(
            CurrentDirectory, ".git");

        /// <summary>
        /// Gets the path to the application's database file.
        /// </summary>
        public static readonly string DatabasePath = Path.Combine(
            RepoToolFolder, "database.db");

        /// <summary>
        /// Gets the path to the application's settings file.
        /// The filename includes the environment name (e.g., settings.Development.json)
        /// unless the environment is Production or not specified, in which case it's settings.json.
        /// </summary>
        public static readonly string SettingsPath = GetSettingsPath();

        /// <summary>
        /// Gets the path to the JSON schema file for the application settings.
        /// </summary>
        public static readonly string SettingsSchemaPath = Path.Combine(
            RepoToolFolder, "settings-schema.json");

        /// <summary>
        /// Determines the appropriate settings file path based on the environment.
        /// </summary>
        /// <returns>The full path to the settings file.</returns>
        private static string GetSettingsPath()
        {
            // Prefer DOTNET_ENVIRONMENT, fall back to ASPNETCORE_ENVIRONMENT
            string? environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            // Use environment-specific file unless it's null, empty, or "Production"
            string settingsFileName = "settings.json";
            if ( !string.IsNullOrEmpty(environmentName)
                && !environmentName.Equals("Production", StringComparison.OrdinalIgnoreCase) )
            {
                settingsFileName = $"settings.{environmentName}.json";
            }

            return Path.Combine(RepoToolFolder, settingsFileName);
        }
    }
}
