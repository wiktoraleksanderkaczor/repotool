using System.ComponentModel;
using Spectre.Console.Cli;

public class CommonSettings : CommandSettings
{
    [CommandOption("--verbose")]
    [Description("Enable verbose output.")]
    public bool Verbose { get; set; } = false;
}