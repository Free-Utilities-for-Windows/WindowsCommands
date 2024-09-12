using System.CommandLine;
using System.CommandLine.Invocation;

namespace WindowsCommands.Commands;

public static class WinCommands
{
    public static Command DownloadImagesCommand()
    {
        var command = new Command("download-images",
            "Download all images from a specified URL. Example: download-images --url \"https://habr.com/ru/articles/818907/\"")
        {
            new Option<string>(
                "--url",
                getDefaultValue: () => "",
                description: "The URL to download images from.")
        };
        command.Handler = CommandHandler.Create<string>(HandleDownloadImagesCommand);
        return command;
    }

    public static Command GetDrivesCommand()
    {
        var command = new Command("get-drives",
            "Get information about all physical drives. If a device ID is provided, get information about logical drives of the specified physical drive. Example: get-drives --device-id \"\\\\.\\PHYSICALDRIVE0\"")
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
        var command = new Command("get-files",
            "Get files from a specified path. Example: get-files --path \"D:\\Games\\Deus Ex HRDC\"")
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
        var command = new Command("get-event-log",
            "Get event log information. Example: get-event-log --log-name \"System\"")
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
        var command = new Command("get-net-interface-stats",
            "Get network interface statistics. Example: get-net-interface-stats --current false")
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
        var command = new Command("get-process-performance",
            "Get process performance information. Example: get-process-performance --process-name \"explorer\"")
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
        var command = new Command("get-user-session",
            "Get user session information. Example: get-user-session --server \"localhost\" --user \"*\"")
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
        var command = new Command("get-web-certificate-info",
            "Get web certificate information. Example: get-web-certificate-info --url \"https://yandex.ru\"")
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
        var command = new Command("start-tcp-server",
            "Start a TCP server on a specified port. Example: start-tcp-server --port 5201")
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
        var command = new Command("start-udp-server",
            "Start a UDP server on a specified port. Example: start-udp-server --port 5201")
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
        var command = new Command("ping-network",
            "Ping a network. Example: ping-network --network \"192.168.1.0\" --timeout 100")
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

    public static Command GetMemorySlotsCommand()
    {
        var command = new Command("get-memory-slots", "Get information about all memory slots");
        command.Handler = CommandHandler.Create(() => MemorySlotInfo.PrintMemorySlots());
        return command;
    }

    public static Command CleanOldTempFilesCommand()
    {
        var command = new Command("clean-old-temp-files",
            "Clean old temporary files that have not been modified in the last 30 days.");
        command.Handler = CommandHandler.Create(TempFileCleaner.CleanOldTempFiles);
        return command;
    }

    public static Command ScanDevicesCommand()
    {
        var command = new Command("scan-devices", "Scan and list all network devices.");
        command.Handler = CommandHandler.Create(() =>
        {
            var devices = DeviceScanner.ScanDevices();
            DeviceScanner.PrintDeviceInformation(devices);
        });
        return command;
    }

    public static Command RunCleanupTasksCommand()
    {
        var command = new Command("cleanup", "Run all cleanup tasks.");
        command.Handler = CommandHandler.Create(() => { CleanerUp.RunCleanupTasks(); });
        return command;
    }

    public static Command ChangeAccessRightsCommand()
    {
        var command = new Command("change-access-rights",
            "Change access rights for a specified file or directory. Example: change-access-rights --path \"D:\\Games\\example.txt\"\"")
        {
            new Option<string>("--path", "The path to the file or directory.")
        };

        command.Handler = CommandHandler.Create<string>((path) =>
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Path is required.");
                return;
            }

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Console.WriteLine("The specified path does not exist.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n1. Show access rights");
                Console.WriteLine("2. Change access rights");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid choice. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ChangeAccessRights.ShowAccessRights(path);
                        break;
                    case 2:
                        ChangeAccessRights.ChangeAccessRightsMenu(path);
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                        break;
                }
            }
        });

        return command;
    }

    public static Command WiFiAnalyzerCommand()
    {
        var command = new Command("wifi-analyzer",
            "Analyze WiFi networks and connect to open networks. Example: wifi-analyzer --action scan --filter all")
        {
            new Option<string>("--action", "The action to perform (scan/connect)."),
            new Option<string>("--ssid", "The SSID of the network to connect to (only for connect action)."),
            new Option<string>("--filter", "Filter for networks (all/open) (only for scan action).")
        };

        command.Handler = CommandHandler.Create<string, string, string>((action, ssid, filter) =>
        {
            if (string.IsNullOrEmpty(action))
            {
                Console.WriteLine("Action is required.");
                return;
            }

            if (action == "scan")
            {
                if (string.IsNullOrEmpty(filter))
                {
                    Console.WriteLine("Filter is required for scan action.");
                    return;
                }

                WiFiAnalyzer.ScanNetworks(filter);
            }
            else if (action == "connect")
            {
                if (string.IsNullOrEmpty(ssid))
                {
                    Console.WriteLine("SSID is required for connect action.");
                    return;
                }

                WiFiAnalyzer.ConnectToNetwork(ssid);
            }
            else
            {
                Console.WriteLine("Invalid action. Please enter 'scan' or 'connect'.");
            }
        });

        return command;
    }

    private static void HandleGetFilesCommand(string path)
    {
        var files = FilesInformation.GetFiles(path);
        foreach (var file in files)
        {
            Console.WriteLine(
                $"Name: {file.Name}, Full Name: {file.FullName}, Type: {file.Type}, Size: {file.Size} GB, Creation Time: {file.CreationTime}, Last Access Time: {file.LastAccessTime}, Last Write Time: {file.LastWriteTime}");
        }
    }

    private static void HandleGetProcessPerformanceCommand(string processName)
    {
        var collector = new ProcessPerformanceCollector();
        var performances = collector.GetProcessPerformance(processName);
        foreach (var performance in performances)
        {
            Console.WriteLine(
                $"Name: {performance.Name}, ProcTime: {performance.ProcTime}, IOps: {performance.IOps}, IObsRead: {performance.IObsRead}, IObsWrite: {performance.IObsWrite}, RunTime: {performance.RunTime}, TotalTime: {performance.TotalTime}, UserTime: {performance.UserTime}, PrivTime: {performance.PrivTime}, WorkingSet: {performance.WorkingSet}, PeakWorkingSet: {performance.PeakWorkingSet}, PageMemory: {performance.PageMemory}, Threads: {performance.Threads}, Handles: {performance.Handles}");
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

    private static void HandleDownloadImagesCommand(string url)
    {
        Task.Run(() => ImageDownloader.DownloadImages(url));
    }
}