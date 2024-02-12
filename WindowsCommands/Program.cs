﻿using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.RegularExpressions;
using WindowsCommands;
using WindowsCommands.Commands;

public class Program
{
    public static void Main(string[] args)
    {
        Printer.StartPage();
        
        var rootCommand = new RootCommand();
        
        rootCommand.AddCommand(WinCommands.GetFilesCommand());
        rootCommand.AddCommand(WinCommands.GetEventLogCommand());
        rootCommand.AddCommand(WinCommands.GetNetInterfaceStatsCommand());
        rootCommand.AddCommand(WinCommands.GetSystemInfoCommand());
        rootCommand.AddCommand(WinCommands.GetMemoryInfoCommand());
        rootCommand.AddCommand(WinCommands.GetCPUInfoCommand());
        rootCommand.AddCommand(WinCommands.GetDriverInfoCommand());
        rootCommand.AddCommand(WinCommands.GetDiskInfoCommand());
        rootCommand.AddCommand(WinCommands.GetIOInfoCommand());
        rootCommand.AddCommand(WinCommands.GetArpTableCommand());
        rootCommand.AddCommand(WinCommands.GetNetAdapterInfoCommand());
        rootCommand.AddCommand(WinCommands.GetNetworkConfigurationCommand());
        rootCommand.AddCommand(WinCommands.MonitorNetworkUtilizationCommand());
        rootCommand.AddCommand(WinCommands.GetProcessPerformanceCommand());
        rootCommand.AddCommand(WinCommands.GetUserSessionCommand());
        rootCommand.AddCommand(WinCommands.GetTemperatureCommand());
        rootCommand.AddCommand(WinCommands.GetVideoCardInfoCommand());
        rootCommand.AddCommand(WinCommands.GetWebCertificateInfoCommand());
        
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

            var inputArgs = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+")
                .Select(m => m.Value.Trim('"'))
                .ToArray();

            rootCommand.InvokeAsync(inputArgs).Wait();
        }
    }
}
