using System;
using System.Management;

namespace WindowsCommands;

public static class DriverInfo
{
    public static void GetDriverInfo()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                Console.WriteLine("Driver Provider Name: {0}", wmi_HD["DriverProviderName"]);
                Console.WriteLine("Friendly Name: {0}", wmi_HD["FriendlyName"]);
                Console.WriteLine("Description: {0}", wmi_HD["Description"]);
                Console.WriteLine("Driver Version: {0}", wmi_HD["DriverVersion"]);
                Console.WriteLine("Driver Date: {0}", wmi_HD["DriverDate"]);
                Console.WriteLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}