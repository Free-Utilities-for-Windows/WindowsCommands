using System.Management;

namespace WindowsCommands;

public static class IOInformation
{
    public static void GetIOInformation()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfDisk_PhysicalDisk");

            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine("Name: {0}", obj["Name"]);
                Console.WriteLine("ReadWriteTime: {0} %", obj["PercentDiskTime"]);
                Console.WriteLine("ReadTime: {0} %", obj["PercentDiskReadTime"]);
                Console.WriteLine("WriteTime: {0} %", obj["PercentDiskWriteTime"]);
                Console.WriteLine("IdleTime: {0} %", obj["PercentIdleTime"]);
                Console.WriteLine("QueueLength: {0}", obj["CurrentDiskQueueLength"]);
                Console.WriteLine("BytesPersec: {0} MByte/Sec", (Convert.ToDouble(obj["DiskBytesPersec"]) / 1024 / 1024).ToString("0.000"));
                Console.WriteLine("ReadBytesPersec: {0} MByte/Sec", (Convert.ToDouble(obj["DiskReadBytesPersec"]) / 1024 / 1024).ToString("0.000"));
                Console.WriteLine("WriteBytesPersec: {0} MByte/Sec", (Convert.ToDouble(obj["DiskWriteBytesPersec"]) / 1024 / 1024).ToString("0.000"));
                Console.WriteLine("IOps: {0}", obj["DiskTransfersPersec"]);
                Console.WriteLine("ReadsIOps: {0}", obj["DiskReadsPersec"]);
                Console.WriteLine("WriteIOps: {0}", obj["DiskWritesPersec"]);
                Console.WriteLine("\n-----------------------------------------");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}