using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepoTool.Commands;
using RepoTool.Commands.Cache;
using RepoTool.Commands.Index;
using RepoTool.Commands.Language;
using RepoTool.Commands.Language.Available;
using RepoTool.Terminal.Commands;
using Spectre.Console.Cli;

namespace RepoTool.Terminal
{
    public static class SpectreRunner
    {
        public static async Task<int> RunSpectreCommands(this IHostBuilder hostBuilder, string[] args)
        {
            TypeRegistrar? registrar = null;
            hostBuilder
                .ConfigureServices(services => 
                {
                    services.AddSingleton(hostBuilder);

                    registrar = new TypeRegistrar(services);
                })
                .Build();

            if (registrar is null)
                throw new InvalidOperationException("TypeRegistrar is not initialized.");

            CommandApp<DefaultCommand> app = new(registrar);

            // Configure the command app
            app.Configure(config =>
            {
                config.SetApplicationName("repotool");
                config.SetInterceptor(new CommandInterceptor());

                // Register commands
                config.AddCommand<StatusCommand>("status")
                    .WithDescription("Show repository status.");

                config.AddBranch("index", index =>
                {
                    index.AddCommand<UpdateIndexCommand>("update")
                        .WithDescription("Update the index with changelogs.");

                    index.AddCommand<ClearIndexCommand>("clear")
                        .WithDescription("Clear the changelog index.");

                    index.AddCommand<ShowIndexCommand>("show")
                        .WithDescription("Show the changelog index.");
                });

                config.AddBranch("cache", cache =>
                {
                    cache.AddCommand<ClearCacheCommand>("clear")
                        .WithDescription("Clear the inference cache.");

                    cache.AddCommand<ShowCacheCommand>("show")
                        .WithDescription("Show the inference cache.");
                });

                config.AddCommand<SearchCommand>("search")
                    .WithDescription("Search for changelogs.");

                config.AddCommand<SummarizeCommand>("summarize")
                    .WithDescription("Summarize a file.");

                config.AddBranch("language", language =>
                {
                    language.AddBranch("available", available =>
                    {
                        available.AddCommand<ListAvailableLanguagesCommand>("list")
                            .WithDescription("List available languages.");

                        available.AddCommand<AddAvailableLanguageCommand>("add")
                            .WithDescription("Add an available language.");
                    });

                    language.AddCommand<ListLanguageCommand>("list")
                        .WithDescription("List supported languages.");

                    language.AddCommand<AddLanguageCommand>("add")
                        .WithDescription("Add a supported language.");

                    language.AddCommand<RemoveLanguageCommand>("remove")
                        .WithDescription("Remove a supported language.");
                });

                config.AddCommand<ParseCommand>("parse")
                    .WithDescription("Parse a file.");

                config.AddCommand<InitCommand>("init")
                    .WithDescription("Initialize the application.");
            });

            return await app.RunAsync(args);
        }
    }
}