// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using Microsoft.Extensions.Hosting;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    internal sealed class DefaultCommand : AsyncCommand<CommonSettings>
    {
        private readonly IHostBuilder _hostBuilder;

        public DefaultCommand(IHostBuilder hostBuilder) => _hostBuilder = hostBuilder;

        public override async Task<int> ExecuteAsync(CommandContext context, CommonSettings settings)
        {
            await _hostBuilder.Build().RunAsync().ConfigureAwait(false);
            return 0;
        }
    }
}
