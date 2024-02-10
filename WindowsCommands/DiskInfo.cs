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
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_LogicalDisk where DriveType=3");

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
}