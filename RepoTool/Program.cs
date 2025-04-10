using Spectre.Console;
using Microsoft.Extensions.Hosting;
using RepoTool.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoTool.Terminal;

namespace RepoTool
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Create a host builder
                HostBuilder builder = new();

                // Configure the host builder
                builder
                    .UseEnvironment(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production")
                    .UseConsoleLifetime()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        if (Directory.Exists(PathConstants.RepoToolFolder))
                        {
                            config.SetBasePath(PathConstants.RepoToolFolder);

                            // Load configuration from settings.json
                            if (context.HostingEnvironment.IsDevelopment())
                            {
                                config.AddJsonFile("settings.Development.json", optional: true, reloadOnChange: true);
                            }
                            else
                            {
                                config.AddJsonFile("settings.json", optional: true, reloadOnChange: true);
                            }
                        }

                        // Load configuration from environment variables
                        // e.g. Repotool_Providers__OpenAi__ApiKey is Providers:OpenAi:ApiKey
                        config.AddEnvironmentVariables(prefix: "Repotool_");

                        // Load configuration from key per file
                        // e.g. Providers__OpenAi__ApiKey is Providers:OpenAi:ApiKey
                        config.AddKeyPerFile(directoryPath: PathConstants.RepoToolFolder, optional: true, reloadOnChange: true);
                    })
                    .ConfigureServices(DependencyInjection.ConfigureServices)
                    .ConfigureLogging((context, logging) =>
                    {
                        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    });

                return await builder.RunSpectreCommands(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return 1;
            }
        }
    }
}
