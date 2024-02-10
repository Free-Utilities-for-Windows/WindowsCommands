using System.Management;

namespace WindowsCommands;

public static class SystemInformation
{
    public static void GetSystemInformation()
    {
        try
        {
            var processor = new ManagementObjectSearcher("select * from Win32_Processor").Get().GetEnumerator();
            processor.MoveNext();
            Console.WriteLine("Processor  : {0}", processor.Current["Name"]);

            var os = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().GetEnumerator();
            os.MoveNext();
            Console.WriteLine("OS : {0}", os.Current["Caption"]);

            var memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory").Get().GetEnumerator();
            memory.MoveNext();
            Console.WriteLine("Memory : {0} GB", Convert.ToInt64(memory.Current["Capacity"]) / 1024 / 1024 / 1024);

            var disk = new ManagementObjectSearcher("select * from Win32_DiskDrive").Get().GetEnumerator();
            while (disk.MoveNext())
            {
                Console.WriteLine("Disk : {0}, Size : {1} GB", disk.Current["Model"],
                    Convert.ToInt64(disk.Current["Size"]) / 1024 / 1024 / 1024);
            }

            var network = new ManagementObjectSearcher("select * from Win32_NetworkAdapter where NetConnectionStatus=2")
                .Get().GetEnumerator();
            while (network.MoveNext())
            {
                Console.WriteLine("Network Adapter : {0}", network.Current["Name"]);
            }

            var videoController = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator();
            videoController.MoveNext();
            Console.WriteLine("Video Controller : {0}", videoController.Current["Name"]);

            var bios = new ManagementObjectSearcher("select * from Win32_BIOS").Get().GetEnumerator();
            bios.MoveNext();
            Console.WriteLine("BIOS : {0}", bios.Current["Manufacturer"]);
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}