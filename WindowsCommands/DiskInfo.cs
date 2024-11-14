using System;
using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class DiskInfo
{
    public static void GetAllDiskInfo()
    {
        try
        {
            GetLogicalDiskInfo();
            GetDiskPartitionInfo();
            GetPhysicalDiskInfo();
            GetSMARTInfo();
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void GetLogicalDiskInfo()
    {
        try
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("select * from Win32_LogicalDisk where DriveType=3");

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Get Logical Disk Info");
            StaticFileLogger.LogInformation("Get Logical Disk Info");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                string info = $"Logical Disk: {wmi_HD["DeviceID"]}\n" +
                              $"FreeSpace: {wmi_HD["FreeSpace"]}\n" +
                              $"Size: {wmi_HD["Size"]}\n" +
                              $"VolumeName: {wmi_HD["VolumeName"]}\n" +
                              $"FileSystem: {wmi_HD["FileSystem"]}";
                Console.WriteLine(info);
                StaticFileLogger.LogInformation(info);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while getting logical disk info: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void GetDiskPartitionInfo()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskPartition");

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Get Disk Partition Info");
            StaticFileLogger.LogInformation("Get Disk Partition Info");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                string info = $"Disk Partition: {wmi_HD["DeviceID"]}\n" +
                              $"Size: {wmi_HD["Size"]}\n" +
                              $"BootPartition: {wmi_HD["BootPartition"]}";
                Console.WriteLine(info);
                StaticFileLogger.LogInformation(info);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while getting disk partition info: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void GetPhysicalDiskInfo()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Get Physical Disk Info");
            StaticFileLogger.LogInformation("Get Physical Disk Info");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                string info = $"Disk Model: {wmi_HD["Model"]}\n" +
                              $"Disk Size: {wmi_HD["Size"]}\n" +
                              $"Partitions: {wmi_HD["Partitions"]}\n" +
                              $"InterfaceType: {wmi_HD["InterfaceType"]}";
                Console.WriteLine(info);
                StaticFileLogger.LogInformation(info);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while getting physical disk info: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public static void GetSMARTInfo()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("\\\\.\\root\\Microsoft\\Windows\\Storage",
                "SELECT * FROM MSFT_PhysicalDisk");

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Get SMART Info");
            StaticFileLogger.LogInformation("Get SMART Info");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                string info = $"DiskName: {queryObj["FriendlyName"]}\n" +
                              $"HealthStatus: {queryObj["HealthStatus"]}\n" +
                              $"OperationalStatus: {string.Join(", ", (UInt16[])queryObj["OperationalStatus"])}\n" +
                              $"MediaType: {queryObj["MediaType"]}\n" +
                              $"BusType: {queryObj["BusType"]}";
                Console.WriteLine(info);
                StaticFileLogger.LogInformation(info);

                var reliabilityCounter = new ManagementObjectSearcher($"\\\\.\\root\\Microsoft\\Windows\\Storage",
                        $"SELECT * FROM MSFT_StorageReliabilityCounter WHERE DeviceId = '{queryObj["DeviceId"]}'").Get()
                    .GetEnumerator();

                if (reliabilityCounter.MoveNext())
                {
                    string reliabilityInfo = $"Temperature: {reliabilityCounter.Current["Temperature"]}\n" +
                                             $"PowerOnHours: {reliabilityCounter.Current["PowerOnHours"]}\n" +
                                             $"StartStopCycleCount: {reliabilityCounter.Current["StartStopCycleCount"]}\n" +
                                             $"FlushLatencyMax: {reliabilityCounter.Current["FlushLatencyMax"]}\n" +
                                             $"LoadUnloadCycleCount: {reliabilityCounter.Current["LoadUnloadCycleCount"]}\n" +
                                             $"ReadErrorsTotal: {reliabilityCounter.Current["ReadErrorsTotal"]}\n" +
                                             $"ReadErrorsCorrected: {reliabilityCounter.Current["ReadErrorsCorrected"]}\n" +
                                             $"ReadErrorsUncorrected: {reliabilityCounter.Current["ReadErrorsUncorrected"]}\n" +
                                             $"ReadLatencyMax: {reliabilityCounter.Current["ReadLatencyMax"]}\n" +
                                             $"WriteErrorsTotal: {reliabilityCounter.Current["WriteErrorsTotal"]}\n" +
                                             $"WriteErrorsCorrected: {reliabilityCounter.Current["WriteErrorsCorrected"]}\n" +
                                             $"WriteErrorsUncorrected: {reliabilityCounter.Current["WriteErrorsUncorrected"]}\n" +
                                             $"WriteLatencyMax: {reliabilityCounter.Current["WriteLatencyMax"]}";
                    Console.WriteLine(reliabilityInfo);
                    StaticFileLogger.LogInformation(reliabilityInfo);
                }
                else
                {
                    string noDataMessage = "No reliability counter data available for this disk.";
                    Console.WriteLine(noDataMessage);
                    StaticFileLogger.LogInformation(noDataMessage);
                }

                Console.WriteLine("\n");
            }
        }
        catch (ManagementException e)
        {
            string errorMessage = "An error occurred while querying for WMI data: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}