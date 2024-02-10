using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.RegularExpressions;
using WindowsCommands;
using WindowsCommands.Commands;

public class Program
{
    public static void Main(string[] args)
    {
        
        /*
        var getFilesCommand = new Command("get-files", "Get files from a specified path")
        {
            new Option<string>(
                "--path", 
                getDefaultValue: () => Environment.CurrentDirectory,
                description: "The path to get files from.")
        };
        getFilesCommand.Handler = CommandHandler.Create<string>(HandleGetFilesCommand);
        
        var getEventLogCommand = new Command("get-event-log", "Get event log information")
        {
            new Option<string>(
                "--log-name", 
                getDefaultValue: () => "",
                description: "The name of the event log to get information from.")
        };
        getEventLogCommand.Handler = CommandHandler.Create<string>(EventLogInfo.GetEvent);
        
        var getNetInterfaceStatsCommand = new Command("get-net-interface-stats", "Get network interface statistics")
        {
            new Option<bool>(
                "--current", 
                getDefaultValue: () => false,
                description: "Whether to get current statistics or total statistics.")
        };
        getNetInterfaceStatsCommand.Handler = CommandHandler.Create<bool>(NetworkInterfaceStats.GetNetworkInterfaceStats);
        
        var getSystemInfoCommand = new Command("get-system-info", "Get system information");
        getSystemInfoCommand.Handler = CommandHandler.Create(SystemInformation.GetSystemInformation);
        
        var getMemoryInfoCommand = new Command("get-memory-info", "Get memory information");
        getMemoryInfoCommand.Handler = CommandHandler.Create(MemoryInformation.GetMemoryInformation);
        
        var getCPUInfoCommand = new Command("get-cpu-info", "Get CPU information");
        getCPUInfoCommand.Handler = CommandHandler.Create(CPUInformation.GetCPUInformation);
        
        var getDriverInfoCommand = new Command("get-driver-info", "Get driver information");
        getDriverInfoCommand.Handler = CommandHandler.Create(DriverInfo.GetDriverInfo);
        
        var getDiskInfoCommand = new Command("get-disk-info", "Get disk information");
        getDiskInfoCommand.Handler = CommandHandler.Create(DiskInfo.GetAllDiskInfo);
        
        var getIOInfoCommand = new Command("get-io-info", "Get IO information");
        getIOInfoCommand.Handler = CommandHandler.Create(IOInformation.GetIOInformation);
        
        var getArpTableCommand = new Command("get-arp-table", "Get ARP table");
        getArpTableCommand.Handler = CommandHandler.Create(ArpTable.GetArpTable);
        
        var getNetAdapterInfoCommand = new Command("get-net-adapter-info", "Get network adapter information");
        getNetAdapterInfoCommand.Handler = CommandHandler.Create(NetworkAdapterInformation.GetNetworkAdapterInformation);
        
        var getNetworkConfigurationCommand = new Command("get-network-config", "Get network configuration");
        getNetworkConfigurationCommand.Handler = CommandHandler.Create(NetworkConfiguration.GetNetworkConfiguration);
        
        var monitorNetworkUtilizationCommand = new Command("monitor-network-utilization", "Monitor network utilization");
        monitorNetworkUtilizationCommand.Handler = CommandHandler.Create(NetworkUtilization.MonitorNetworkUtilization);
        */
        
        
        var rootCommand = new RootCommand();
        
        /*
        rootCommand.AddCommand(getFilesCommand);
        rootCommand.AddCommand(getSystemInfoCommand);
        rootCommand.AddCommand(getMemoryInfoCommand);
        rootCommand.AddCommand(getCPUInfoCommand);
        rootCommand.AddCommand(getArpTableCommand);
        rootCommand.AddCommand(getDiskInfoCommand);
        rootCommand.AddCommand(getDriverInfoCommand);
        rootCommand.AddCommand(getEventLogCommand);
        rootCommand.AddCommand(getIOInfoCommand);
        rootCommand.AddCommand(getNetAdapterInfoCommand);
        rootCommand.AddCommand(getNetInterfaceStatsCommand);
        rootCommand.AddCommand(getNetworkConfigurationCommand);
        rootCommand.AddCommand(monitorNetworkUtilizationCommand);
        */
        
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

    /*
    private static void HandleGetFilesCommand(string path)
    {
        var files = FilesInformation.GetFiles(path);
        foreach (var file in files)
        {
            Console.WriteLine($"Name: {file.Name}, Full Name: {file.FullName}, Type: {file.Type}, Size: {file.Size} GB, Creation Time: {file.CreationTime}, Last Access Time: {file.LastAccessTime}, Last Write Time: {file.LastWriteTime}");
        }
    }
    */
}
