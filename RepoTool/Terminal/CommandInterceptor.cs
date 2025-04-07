using RepoTool.Constants;
using Spectre.Console.Cli;

internal class CommandInterceptor : ICommandInterceptor
{
    // <summary>
    // Intercepts the command settings and performs any necessary init checks.
    // Must be non-static to be called by Spectre.Console.
    // </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The command settings.</param>
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (context.Name != "init")
        {
            // Ensure the directory contains a git repo
            if (!Directory.Exists(PathConstants.GitFolder))
            {
                Console.WriteLine($"Directory '{PathConstants.CurrentDirectory}' does not contain git repository.");
                Environment.Exit(1);
            }

            if (!Directory.Exists(PathConstants.RepoToolFolder)
                || !File.Exists(PathConstants.DatabasePath))
            {
                Console.WriteLine("Must run initialization command first");
                Environment.Exit(1);
            }
        }
    }
}