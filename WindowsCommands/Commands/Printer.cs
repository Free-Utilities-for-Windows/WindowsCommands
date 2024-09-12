using System.CommandLine;

namespace WindowsCommands.Commands;

public static class Printer
{
    public static void StartPage()
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        string title = @"
__        ___           _                             
\ \      / (_)_ __   __| | _____      _____           
 \ \ /\ / /| | '_ \ / _` |/ _ \ \ /\ / / __|          
  \ V  V / | | | | | (_| | (_) \ V  V /\__ \          
  _\_/\_/  |_|_| |_|\__,_|\___/ \_/\_/ |___/    _     
 / ___|___  _ __ ___  _ __ ___   __ _ _ __   __| |___ 
| |   / _ \| '_ ` _ \| '_ ` _ \ / _` | '_ \ / _` / __|
| |__| (_) | | | | | | | | | | | (_| | | | | (_| \__ \
 \____\___/|_| |_| |_|_| |_| |_|\__,_|_| |_|\__,_|___/
                                                      
";
        Console.WriteLine(title);

        string text = "List all available commands:\n";
        Console.WriteLine(text);

        Console.ResetColor();
    }
    
    public static void AvailableCommands()
    {
        Console.ForegroundColor = ConsoleColor.White;
    
        var commands = new List<Command>
        {
            WinCommands.GetFilesCommand(),
            WinCommands.GetEventLogCommand(),
            WinCommands.GetDrivesCommand(),
            WinCommands.GetNetInterfaceStatsCommand(),
            WinCommands.GetProcessPerformanceCommand(),
            WinCommands.GetUserSessionCommand(),
            WinCommands.GetWebCertificateInfoCommand(),
            WinCommands.StartTcpServerCommand(),
            WinCommands.StartUdpServerCommand(),
            WinCommands.PingNetworkCommand(),
            WinCommands.GetSystemInfoCommand(),
            WinCommands.GetMemoryInfoCommand(),
            WinCommands.GetMemorySlotsCommand(),
            WinCommands.GetCPUInfoCommand(),
            WinCommands.GetDriverInfoCommand(),
            WinCommands.GetDiskInfoCommand(),
            WinCommands.GetIOInfoCommand(),
            WinCommands.GetArpTableCommand(),
            WinCommands.GetNetAdapterInfoCommand(),
            WinCommands.GetNetworkConfigurationCommand(),
            WinCommands.GetTemperatureCommand(),
            WinCommands.GetVideoCardInfoCommand(),
            WinCommands.GetWindowsUpdateInfoCommand(),
            WinCommands.GetBatteryInfoCommand(),
            WinCommands.MonitorNetworkUtilizationCommand(),
            WinCommands.CleanOldTempFilesCommand(),
            WinCommands.DownloadImagesCommand(),
            WinCommands.ScanDevicesCommand(),
            WinCommands.RunCleanupTasksCommand(),
            WinCommands.ChangeAccessRightsCommand(),
            WinCommands.WiFiAnalyzerCommand()
        };

        foreach (var command in commands)
        {
            Console.WriteLine($"Command: {command.Name}");
            Console.WriteLine($"Description: {command.Description}");
    
            foreach (var option in command.Options)
            {
                Console.WriteLine($"Option: {option.Aliases.First()}");
            }

            Console.WriteLine();
        }
    }
}