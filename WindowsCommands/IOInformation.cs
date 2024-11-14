using System.Management;
using WindowsCommands.Logger;

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
                string ioInfo = $"Name: {obj["Name"]}\n" +
                                $"ReadWriteTime: {obj["PercentDiskTime"]} %\n" +
                                $"ReadTime: {obj["PercentDiskReadTime"]} %\n" +
                                $"WriteTime: {obj["PercentDiskWriteTime"]} %\n" +
                                $"IdleTime: {obj["PercentIdleTime"]} %\n" +
                                $"QueueLength: {obj["CurrentDiskQueueLength"]}\n" +
                                $"BytesPersec: {(Convert.ToDouble(obj["DiskBytesPersec"]) / 1024 / 1024).ToString("0.000")} MByte/Sec\n" +
                                $"ReadBytesPersec: {(Convert.ToDouble(obj["DiskReadBytesPersec"]) / 1024 / 1024).ToString("0.000")} MByte/Sec\n" +
                                $"WriteBytesPersec: {(Convert.ToDouble(obj["DiskWriteBytesPersec"]) / 1024 / 1024).ToString("0.000")} MByte/Sec\n" +
                                $"IOps: {obj["DiskTransfersPersec"]}\n" +
                                $"ReadsIOps: {obj["DiskReadsPersec"]}\n" +
                                $"WriteIOps: {obj["DiskWritesPersec"]}\n" +
                                "\n-----------------------------------------";
                Console.WriteLine(ioInfo);
                StaticFileLogger.LogInformation(ioInfo);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}