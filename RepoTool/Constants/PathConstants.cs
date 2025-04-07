namespace RepoTool.Constants
{
    public static class PathConstants
    {
        public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        public static readonly string UserRepoToolFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".repotool");

        public static readonly string UserRepoToolSchemaFolder = Path.Combine(
            UserRepoToolFolder, ".repotool", "schemas");

        public static readonly string RepoToolFolder = Path.Combine(
            CurrentDirectory, ".repotool");

        public static readonly string GitFolder = Path.Combine(
            CurrentDirectory, ".git");
        
        public static readonly string DatabasePath = Path.Combine(
            RepoToolFolder, "database.db");
    }
}