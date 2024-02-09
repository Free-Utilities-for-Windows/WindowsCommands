using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.RegularExpressions;
using WindowsCommands;

public class Program
{
    public static void Main(string[] args)
    {
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
        
        var getSystemInfoCommand = new Command("get-system-info", "Get system information");
        getSystemInfoCommand.Handler = CommandHandler.Create(SystemInformation.GetSystemInformation);
        
        var getMemoryInfoCommand = new Command("get-memory-info", "Get memory information");
        getMemoryInfoCommand.Handler = CommandHandler.Create(MemoryInformation.GetMemoryInformation);
        
        var getCPUInfoCommand = new Command("get-cpu-info", "Get CPU information");
        getCPUInfoCommand.Handler = CommandHandler.Create(CPUInformation.GetCPUInformation);
        
        var getArpTableCommand = new Command("get-arp-table", "Get ARP table");
        getArpTableCommand.Handler = CommandHandler.Create(ArpTable.GetArpTable);
        
        var getDiskInfoCommand = new Command("get-disk-info", "Get disk information");
        getDiskInfoCommand.Handler = CommandHandler.Create(DiskInfo.GetAllDiskInfo);
        
        var getDriverInfoCommand = new Command("get-driver-info", "Get driver information");
        getDriverInfoCommand.Handler = CommandHandler.Create(DriverInfo.GetDriverInfo);
        
        var rootCommand = new RootCommand();
        rootCommand.AddCommand(getFilesCommand);
        rootCommand.AddCommand(getSystemInfoCommand);
        rootCommand.AddCommand(getMemoryInfoCommand);
        rootCommand.AddCommand(getCPUInfoCommand);
        rootCommand.AddCommand(getArpTableCommand);
        rootCommand.AddCommand(getDiskInfoCommand);
        rootCommand.AddCommand(getDriverInfoCommand);
        rootCommand.AddCommand(getEventLogCommand);

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

    private static void HandleGetFilesCommand(string path)
    {
        var files = FilesInformation.GetFiles(path);
        foreach (var file in files)
        {
            Console.WriteLine($"Name: {file.Name}, Full Name: {file.FullName}, Type: {file.Type}, Size: {file.Size} GB, Creation Time: {file.CreationTime}, Last Access Time: {file.LastAccessTime}, Last Write Time: {file.LastWriteTime}");
        }
    }
}
