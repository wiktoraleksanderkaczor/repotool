// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepoTool.Constants;
using RepoTool.Terminal;
using Spectre.Console;

namespace RepoTool
{
    internal sealed class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Create a host builder
                HostBuilder builder = new();

                // Configure the host builder
                builder = (HostBuilder)builder
                    .UseEnvironment(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production")
                    .UseConsoleLifetime()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        if ( Directory.Exists(PathConstants.RepoToolFolder) )
                        {
                            config = config.SetBasePath(PathConstants.RepoToolFolder);

                            // Load configuration from settings.json
                            config = config.AddJsonFile("settings.json", optional: true, reloadOnChange: true);

                            // Override for development
                            if ( context.HostingEnvironment.IsDevelopment() )
                            {
                                config = config.AddJsonFile("settings.Development.json", optional: true, reloadOnChange: true);
                            }
                        }

                        // Load configuration from environment variables
                        // e.g. Repotool_Providers__OpenAI__ApiKey is Providers:OpenAI:ApiKey
                        config = config.AddEnvironmentVariables(prefix: "Repotool_");

                        // Load configuration from key per file
                        // e.g. Providers__OpenAI__ApiKey is Providers:OpenAI:ApiKey
                        config = config.AddKeyPerFile(directoryPath: PathConstants.RepoToolFolder, optional: true, reloadOnChange: true);
                    })
                    .ConfigureServices(DependencyInjection.ConfigureServices)
                    .ConfigureLogging((context, logging) =>
                    {
                        logging = logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                        logging = logging.AddConsole();
                    });

                return await builder.RunSpectreCommands(args).ConfigureAwait(false);
            }
            catch ( Exception ex )
            {
                AnsiConsole.WriteException(ex);
                return 1;
                throw;
            }
        }
    }
}
