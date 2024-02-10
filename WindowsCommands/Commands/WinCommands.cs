using System.CommandLine;
using System.CommandLine.Invocation;

namespace WindowsCommands.Commands;

public static class WinCommands
{
    public static Command GetFilesCommand()
    {
        var command = new Command("get-files", "Get files from a specified path")
        {
            new Option<string>(
                "--path", 
                getDefaultValue: () => Environment.CurrentDirectory,
                description: "The path to get files from.")
        };
        command.Handler = CommandHandler.Create<string>(HandleGetFilesCommand);
        return command;
    }
    
    public static Command GetEventLogCommand()
    {
        var command = new Command("get-event-log", "Get event log information")
        {
            new Option<string>(
                "--log-name", 
                getDefaultValue: () => "",
                description: "The name of the event log to get information from.")
        };
        command.Handler = CommandHandler.Create<string>(EventLogInfo.GetEvent);
        return command;
    }
    
    public static Command GetNetInterfaceStatsCommand()
    {
        var command = new Command("get-net-interface-stats", "Get network interface statistics")
        {
            new Option<bool>(
                "--current", 
                getDefaultValue: () => false,
                description: "Whether to get current statistics or total statistics.")
        };
        command.Handler = CommandHandler.Create<bool>(NetworkInterfaceStats.GetNetworkInterfaceStats);
        return command;
    }
    
    public static Command GetSystemInfoCommand()
    {
        var command = new Command("get-system-info", "Get system information");
        command.Handler = CommandHandler.Create(SystemInformation.GetSystemInformation);
        return command;
    }
    
    public static Command GetMemoryInfoCommand()
    {
        var command = new Command("get-memory-info", "Get memory information");
        command.Handler = CommandHandler.Create(MemoryInformation.GetMemoryInformation);
        return command;
    }
    
    public static Command GetCPUInfoCommand()
    {
        var command = new Command("get-cpu-info", "Get CPU information");
        command.Handler = CommandHandler.Create(CPUInformation.GetCPUInformation);
        return command;
    }
    
    public static Command GetDriverInfoCommand()
    {
        var command = new Command("get-driver-info", "Get driver information");
        command.Handler = CommandHandler.Create(DriverInfo.GetDriverInfo);
        return command;
    }
    
    public static Command GetDiskInfoCommand()
    {
        var command = new Command("get-disk-info", "Get disk information");
        command.Handler = CommandHandler.Create(DiskInfo.GetAllDiskInfo);
        return command;
    }
    
    public static Command GetIOInfoCommand()
    {
        var command = new Command("get-io-info", "Get IO information");
        command.Handler = CommandHandler.Create(IOInformation.GetIOInformation);
        return command;
    }

    public static Command GetArpTableCommand()
    {
        var command = new Command("get-arp-table", "Get ARP table");
        command.Handler = CommandHandler.Create(ArpTable.GetArpTable);
        return command;
    }

    public static Command GetNetAdapterInfoCommand()
    {
        var command = new Command("get-net-adapter-info", "Get network adapter information");
        command.Handler = CommandHandler.Create(NetworkAdapterInformation.GetNetworkAdapterInformation);
        return command;
    }

    public static Command GetNetworkConfigurationCommand()
    {
        var command = new Command("get-network-config", "Get network configuration");
        command.Handler = CommandHandler.Create(NetworkConfiguration.GetNetworkConfiguration);
        return command;
    }

    public static Command MonitorNetworkUtilizationCommand()
    {
        var command = new Command("monitor-network-utilization", "Monitor network utilization");
        command.Handler = CommandHandler.Create(NetworkUtilization.MonitorNetworkUtilization);
        return command;
    }
    
    
    private static void HandleGetFilesCommand(string path)
    {
        var files = FilesInformation.GetFiles(path);
        foreach (var file in files)
        {
            Console.WriteLine($"Name: {file.Name}, Full Name: {file.FullName}, Type: {file.Type}, Size: {file.Size} GB, Creation Time: {file.CreationTime}, Last Access Time: {file.LastAccessTime}, Last Write Time: {file.LastWriteTime}");
        }
    }
}