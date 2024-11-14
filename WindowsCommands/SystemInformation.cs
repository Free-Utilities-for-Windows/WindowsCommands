using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class SystemInformation
{
    public static void GetSystemInformation()
    {
        try
        {
            var processor = new ManagementObjectSearcher("select * from Win32_Processor").Get().GetEnumerator();
            processor.MoveNext();
            string processorInfo = $"Processor: {processor.Current["Name"]}";
            Console.WriteLine(processorInfo);
            StaticFileLogger.LogInformation(processorInfo);

            var os = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().GetEnumerator();
            os.MoveNext();
            string osInfo = $"OS: {os.Current["Caption"]}";
            Console.WriteLine(osInfo);
            StaticFileLogger.LogInformation(osInfo);

            var memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory").Get().GetEnumerator();
            memory.MoveNext();
            string memoryInfo = $"Memory: {Convert.ToInt64(memory.Current["Capacity"]) / 1024 / 1024 / 1024} GB";
            Console.WriteLine(memoryInfo);
            StaticFileLogger.LogInformation(memoryInfo);

            var disk = new ManagementObjectSearcher("select * from Win32_DiskDrive").Get().GetEnumerator();
            while (disk.MoveNext())
            {
                string diskInfo = $"Disk: {disk.Current["Model"]}, Size: {Convert.ToInt64(disk.Current["Size"]) / 1024 / 1024 / 1024} GB";
                Console.WriteLine(diskInfo);
                StaticFileLogger.LogInformation(diskInfo);
            }

            var network = new ManagementObjectSearcher("select * from Win32_NetworkAdapter where NetConnectionStatus=2")
                .Get().GetEnumerator();
            while (network.MoveNext())
            {
                string networkInfo = $"Network Adapter: {network.Current["Name"]}";
                Console.WriteLine(networkInfo);
                StaticFileLogger.LogInformation(networkInfo);
            }

            var videoController = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator();
            videoController.MoveNext();
            string videoControllerInfo = $"Video Controller: {videoController.Current["Name"]}";
            Console.WriteLine(videoControllerInfo);
            StaticFileLogger.LogInformation(videoControllerInfo);

            var bios = new ManagementObjectSearcher("select * from Win32_BIOS").Get().GetEnumerator();
            bios.MoveNext();
            string biosInfo = $"BIOS: {bios.Current["Manufacturer"]}";
            Console.WriteLine(biosInfo);
            StaticFileLogger.LogInformation(biosInfo);
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}