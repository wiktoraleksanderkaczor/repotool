// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepoTool.Terminal.Commands;
using RepoTool.Terminal.Commands.Cache;
using RepoTool.Terminal.Commands.Index;
using RepoTool.Terminal.Commands.Language;
using RepoTool.Terminal.Commands.Language.Available;
using Spectre.Console.Cli;

namespace RepoTool.Terminal
{
    internal static class SpectreRunner
    {
        public static async Task<int> RunSpectreCommands(this IHostBuilder hostBuilder, string[] args)
        {
            TypeRegistrar? registrar = null;
            IHost host = hostBuilder
                .ConfigureServices(services =>
                {
                    services = services.AddSingleton(hostBuilder);

                    registrar = new TypeRegistrar(services);
                })
                .Build();

            if ( registrar is null )
            {
                throw new InvalidOperationException("TypeRegistrar is not initialized.");
            }

            CommandApp<DefaultCommand> app = new(registrar);

            // Configure the command app
            app.Configure(config =>
            {
                config = config.SetApplicationName("repotool");
                config = config.SetInterceptor(new CommandInterceptor());

                // Register commands
                _ = config.AddCommand<StatusCommand>("status")
                    .WithDescription("Show repository status.");

                _ = config.AddBranch("index", index =>
                {
                    _ = index.AddCommand<UpdateIndexCommand>("update")
                        .WithDescription("Update the index with changelogs.");

                    _ = index.AddCommand<ClearIndexCommand>("clear")
                        .WithDescription("Clear the changelog index.");

                    _ = index.AddCommand<ShowIndexCommand>("show")
                        .WithDescription("Show the changelog index.");
                });

                _ = config.AddBranch("cache", cache =>
                {
                    _ = cache.AddCommand<ClearCacheCommand>("clear")
                        .WithDescription("Clear the inference cache.");

                    _ = cache.AddCommand<ShowCacheCommand>("show")
                        .WithDescription("Show the inference cache.");
                });

                _ = config.AddCommand<SearchCommand>("search")
                    .WithDescription("Search for changelogs.");

                _ = config.AddCommand<SummarizeCommand>("summarize")
                    .WithDescription("Summarize a file.");

                _ = config.AddBranch("language", language =>
                {
                    _ = language.AddBranch("available", available =>
                    {
                        _ = available.AddCommand<ListAvailableLanguagesCommand>("list")
                            .WithDescription("List available languages.");

                        _ = available.AddCommand<AddAvailableLanguageCommand>("add")
                            .WithDescription("Add an available language.");
                    });

                    _ = language.AddCommand<ListLanguageCommand>("list")
                        .WithDescription("List supported languages.");

                    _ = language.AddCommand<AddLanguageCommand>("add")
                        .WithDescription("Add a supported language.");

                    _ = language.AddCommand<RemoveLanguageCommand>("remove")
                        .WithDescription("Remove a supported language.");
                });

                _ = config.AddCommand<ParseCommand>("parse")
                    .WithDescription("Parse a file.");

                _ = config.AddCommand<InitCommand>("init")
                    .WithDescription("Initialize the application.");
            });

            return await app.RunAsync(args).ConfigureAwait(false);
        }
    }
}
