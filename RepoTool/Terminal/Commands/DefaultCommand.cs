using Microsoft.Extensions.Hosting;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    public class DefaultCommand : AsyncCommand<CommonSettings>
    {
        private readonly IHostBuilder _hostBuilder;

        public DefaultCommand(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, CommonSettings settings)
        {
            await _hostBuilder.Build().RunAsync();
            return 0;
        }
    }
}