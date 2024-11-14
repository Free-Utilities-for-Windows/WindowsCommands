using WindowsCommands.Logger;

namespace WindowsCommands;

using System;
using System.Management;

public static class NetworkAdapterInformation
{
    public static void GetNetworkAdapterInformation()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE");

            foreach (ManagementObject obj in searcher.Get())
            {
                string adapterInfo = $"Description: {obj["Description"]}\n" +
                                     $"DHCPEnabled: {obj["DHCPEnabled"]}\n" +
                                     $"DHCPLeaseObtained: {ConvertToDateTime(obj["DHCPLeaseObtained"])}\n" +
                                     $"DHCPLeaseExpires: {ConvertToDateTime(obj["DHCPLeaseExpires"])}\n" +
                                     $"DHCPServer: {obj["DHCPServer"]}\n" +
                                     $"IPAddress: {String.Join(", ", (string[])obj["IPAddress"])}\n" +
                                     $"DefaultIPGateway: {String.Join(", ", (string[])obj["DefaultIPGateway"])}\n" +
                                     $"IPSubnet: {String.Join(", ", (string[])obj["IPSubnet"])}\n" +
                                     $"MACAddress: {obj["MACAddress"]}\n" +
                                     "\n-----------------------------------------";
                Console.WriteLine(adapterInfo);
                StaticFileLogger.LogInformation(adapterInfo);
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

    private static DateTime ConvertToDateTime(object unconvertedTime)
    {
        try
        {
            if (unconvertedTime != null)
            {
                return ManagementDateTimeConverter.ToDateTime(unconvertedTime.ToString());
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while converting time: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
        return DateTime.MinValue;
    }
}