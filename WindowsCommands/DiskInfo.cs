using System;
using System.Management;

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
            Console.WriteLine("An error occurred: " + e.Message);
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

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                Console.WriteLine("Logical Disk: {0}", wmi_HD["DeviceID"]);
                Console.WriteLine("FreeSpace: {0}", wmi_HD["FreeSpace"]);
                Console.WriteLine("Size: {0}", wmi_HD["Size"]);
                Console.WriteLine("VolumeName: {0}", wmi_HD["VolumeName"]);
                Console.WriteLine("FileSystem: {0}", wmi_HD["FileSystem"]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while getting logical disk info: " + e.Message);
        }
    }

    private static void GetDiskPartitionInfo()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskPartition");

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Get Disk Partition Info");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                Console.WriteLine("Disk Partition: {0}", wmi_HD["DeviceID"]);
                Console.WriteLine("Size: {0}", wmi_HD["Size"]);
                Console.WriteLine("BootPartition: {0}", wmi_HD["BootPartition"]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while getting disk partition info: " + e.Message);
        }
    }

    private static void GetPhysicalDiskInfo()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Get Physical Disk Info");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                Console.WriteLine("Disk Model: {0}", wmi_HD["Model"]);
                Console.WriteLine("Disk Size: {0}", wmi_HD["Size"]);
                Console.WriteLine("Partitions: {0}", wmi_HD["Partitions"]);
                Console.WriteLine("InterfaceType: {0}", wmi_HD["InterfaceType"]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while getting physical disk info: " + e.Message);
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

            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine($"DiskName: {queryObj["FriendlyName"]}");
                Console.WriteLine($"HealthStatus: {queryObj["HealthStatus"]}");
                Console.WriteLine($"OperationalStatus: {string.Join(", ", (UInt16[])queryObj["OperationalStatus"])}");
                Console.WriteLine($"MediaType: {queryObj["MediaType"]}");
                Console.WriteLine($"BusType: {queryObj["BusType"]}");

                var reliabilityCounter = new ManagementObjectSearcher($"\\\\.\\root\\Microsoft\\Windows\\Storage",
                        $"SELECT * FROM MSFT_StorageReliabilityCounter WHERE DeviceId = '{queryObj["DeviceId"]}'").Get()
                    .GetEnumerator();

                if (reliabilityCounter.MoveNext())
                {
                    Console.WriteLine($"Temperature: {reliabilityCounter.Current["Temperature"]}");
                    Console.WriteLine($"PowerOnHours: {reliabilityCounter.Current["PowerOnHours"]}");
                    Console.WriteLine($"StartStopCycleCount: {reliabilityCounter.Current["StartStopCycleCount"]}");
                    Console.WriteLine($"FlushLatencyMax: {reliabilityCounter.Current["FlushLatencyMax"]}");
                    Console.WriteLine($"LoadUnloadCycleCount: {reliabilityCounter.Current["LoadUnloadCycleCount"]}");
                    Console.WriteLine($"ReadErrorsTotal: {reliabilityCounter.Current["ReadErrorsTotal"]}");
                    Console.WriteLine($"ReadErrorsCorrected: {reliabilityCounter.Current["ReadErrorsCorrected"]}");
                    Console.WriteLine($"ReadErrorsUncorrected: {reliabilityCounter.Current["ReadErrorsUncorrected"]}");
                    Console.WriteLine($"ReadLatencyMax: {reliabilityCounter.Current["ReadLatencyMax"]}");
                    Console.WriteLine($"WriteErrorsTotal: {reliabilityCounter.Current["WriteErrorsTotal"]}");
                    Console.WriteLine($"WriteErrorsCorrected: {reliabilityCounter.Current["WriteErrorsCorrected"]}");
                    Console.WriteLine(
                        $"WriteErrorsUncorrected: {reliabilityCounter.Current["WriteErrorsUncorrected"]}");
                    Console.WriteLine($"WriteLatencyMax: {reliabilityCounter.Current["WriteLatencyMax"]}");
                }
                else
                {
                    Console.WriteLine("No reliability counter data available for this disk.");
                }

                Console.WriteLine("\n");
            }
        }
        catch (ManagementException e)
        {
            Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}