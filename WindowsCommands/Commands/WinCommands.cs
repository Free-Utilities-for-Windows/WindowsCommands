using System.CommandLine;
using System.CommandLine.Invocation;

namespace WindowsCommands.Commands;

public static class WinCommands
{
    public static Command GetDrivesCommand()
    {
        var command = new Command("get-drives", "Get information about all physical drives. If a device ID is provided, get information about logical drives of the specified physical drive. Example: get-drives --device-id \"\\\\.\\PHYSICALDRIVE0\"")
        {
            new Option<string>(
                "--device-id",
                getDefaultValue: () => "",
                description: "The device ID of the physical drive to get logical drives from."),
        };
        command.Handler = CommandHandler.Create<string>(HandleGetDrivesCommand);
        return command;
    }
    
    public static Command GetFilesCommand()
    {
        var command = new Command("get-files", "Get files from a specified path. Example: get-files --path \"D:\\Games\\Deus Ex HRDC\"")
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
        var command = new Command("get-event-log", "Get event log information. Example: get-event-log --log-name \"System\"")
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
        var command = new Command("get-net-interface-stats", "Get network interface statistics. Example: get-net-interface-stats --current false")
        {
            new Option<bool>(
                "--current", 
                getDefaultValue: () => false,
                description: "Whether to get current statistics or total statistics.")
        };
        command.Handler = CommandHandler.Create<bool>(NetworkInterfaceStats.GetNetworkInterfaceStats);
        return command;
    }
    
    public static Command GetProcessPerformanceCommand()
    {
        var command = new Command("get-process-performance", "Get process performance information. Example: get-process-performance --process-name \"explorer\"")
        {
            new Option<string>(
                "--process-name", 
                getDefaultValue: () => "explorer",
                description: "The name of the process to get performance information from")
        };
        command.Handler = CommandHandler.Create<string>(HandleGetProcessPerformanceCommand);
        return command;
    }
    
    public static Command GetUserSessionCommand()
    {
        var command = new Command("get-user-session", "Get user session information. Example: get-user-session --server \"localhost\" --user \"*\"")
        {
            new Option<string>(
                "--server", 
                getDefaultValue: () => "localhost",
                description: "The server to get user session information from."),
            new Option<string>(
                "--user", 
                getDefaultValue: () => "*",
                description: "The user to get session information for.")
        };
        command.Handler = CommandHandler.Create<string, string>(UserSessionQuery.GetQuery);
        return command;
    }
    
    public static Command GetWebCertificateInfoCommand()
    {
        var command = new Command("get-web-certificate-info", "Get web certificate information. Example: get-web-certificate-info --url \"https://yandex.ru\"")
        {
            new Option<string>(
                "--url", 
                getDefaultValue: () => "https://yandex.ru",
                description: "The URL to get the certificate information from.")
        };
        command.Handler = CommandHandler.Create<string>(WebCertificateInformation.GetWebCertificateInfo);
        return command;
    }
    
    public static Command StartTcpServerCommand()
    {
        var command = new Command("start-tcp-server", "Start a TCP server on a specified port. Example: start-tcp-server --port 5201")
        {
            new Option<int>(
                "--port", 
                getDefaultValue: () => 5201,
                description: "The port to start the server on. Example: start-tcp-server --port 5201")
        };
        command.Handler = CommandHandler.Create<int>(TcpServer.StartTcpServer);
        return command;
    }
    
    public static Command StartUdpServerCommand()
    {
        var command = new Command("start-udp-server", "Start a UDP server on a specified port. Example: start-udp-server --port 5201")
        {
            new Option<int>(
                "--port", 
                getDefaultValue: () => 5201,
                description: "The port to start the server on.")
        };
        command.Handler = CommandHandler.Create<int>(UdpServer.StartUdpServer);
        return command;
    }
    
    public static Command PingNetworkCommand()
    {
        var command = new Command("ping-network", "Ping a network. Example: ping-network --network \"192.168.1.0\" --timeout 100")
        {
            new Option<string>(
                "--network", 
                getDefaultValue: () => "192.168.1.0",
                description: "The network to ping."),
            new Option<int>(
                "--timeout", 
                getDefaultValue: () => 100,
                description: "The timeout for each ping in milliseconds.")
        };
        command.Handler = CommandHandler.Create<string, int>(NetworkPing.PingNetwork);
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
    
    public static Command GetTemperatureCommand()
    {
        var command = new Command("get-temperature", "Get system temperature information");
        command.Handler = CommandHandler.Create(TemperatureInfo.GetTemperature);
        return command;
    }
    
    public static Command GetVideoCardInfoCommand()
    {
        var command = new Command("get-video-card-info", "Get video card information");
        command.Handler = CommandHandler.Create(VideoCardInformation.GetVideoCardInfo);
        return command;
    }
    
    public static Command GetWindowsUpdateInfoCommand()
    {
        var command = new Command("get-windows-update-info", "Get Windows update information");
        command.Handler = CommandHandler.Create(WindowsUpdateInformation.GetWindowsUpdateInfo);
        return command;
    }
    
    public static Command GetBatteryInfoCommand()
    {
        var command = new Command("get-battery-info", "Get Battery information");
        command.Handler = CommandHandler.Create(Battery.GetBatteryInfo);
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
    
    private static void HandleGetProcessPerformanceCommand(string processName)
    {
        var collector = new ProcessPerformanceCollector();
        var performances = collector.GetProcessPerformance(processName);
        foreach (var performance in performances)
        {
            Console.WriteLine($"Name: {performance.Name}, ProcTime: {performance.ProcTime}, IOps: {performance.IOps}, IObsRead: {performance.IObsRead}, IObsWrite: {performance.IObsWrite}, RunTime: {performance.RunTime}, TotalTime: {performance.TotalTime}, UserTime: {performance.UserTime}, PrivTime: {performance.PrivTime}, WorkingSet: {performance.WorkingSet}, PeakWorkingSet: {performance.PeakWorkingSet}, PageMemory: {performance.PageMemory}, Threads: {performance.Threads}, Handles: {performance.Handles}");
        }
    }
    
    private static void HandleGetDrivesCommand(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            DiskExplorer.GetDrives();
        }
        else
        {
            DiskExplorer.GetLogicalDrives(deviceId);
        }
    }
}