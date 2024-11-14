using System;
using System.Management;
using WindowsCommands.Logger;

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
                string driverInfo = $"Driver Provider Name: {wmi_HD["DriverProviderName"]}\n" +
                                    $"Friendly Name: {wmi_HD["FriendlyName"]}\n" +
                                    $"Description: {wmi_HD["Description"]}\n" +
                                    $"Driver Version: {wmi_HD["DriverVersion"]}\n" +
                                    $"Driver Date: {wmi_HD["DriverDate"]}\n";
                Console.WriteLine(driverInfo);
                StaticFileLogger.LogInformation(driverInfo);
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