using System;
using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class NetworkConfiguration
{
    public static void GetNetworkConfiguration()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE");

            foreach (ManagementObject obj in searcher.Get())
            {
                string networkConfigInfo = $"Description: {obj["Description"]}\n" +
                                           $"IPAddress: {(obj["IPAddress"] != null ? String.Join(", ", (string[])obj["IPAddress"]) : "N/A")}\n" +
                                           $"GatewayDefault: {(obj["DefaultIPGateway"] != null ? String.Join(", ", (string[])obj["DefaultIPGateway"]) : "N/A")}\n" +
                                           $"Subnet: {(obj["IPSubnet"] != null ? String.Join(", ", (string[])obj["IPSubnet"]) : "N/A")}\n" +
                                           $"DNSServer: {(obj["DNSServerSearchOrder"] != null ? String.Join(", ", (string[])obj["DNSServerSearchOrder"]) : "N/A")}\n" +
                                           $"MACAddress: {obj["MACAddress"]}\n" +
                                           $"DHCPEnabled: {obj["DHCPEnabled"]}\n" +
                                           $"DHCPServer: {obj["DHCPServer"]}\n" +
                                           $"DHCPLeaseObtained: {ConvertToDateTime(obj["DHCPLeaseObtained"])}\n" +
                                           $"DHCPLeaseExpires: {ConvertToDateTime(obj["DHCPLeaseExpires"])}\n" +
                                           "\n-----------------------------------------";
                Console.WriteLine(networkConfigInfo);
                StaticFileLogger.LogInformation(networkConfigInfo);
            }
        }
        catch (Exception ex)
        {
            string errorMessage = "An error occurred: " + ex.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static DateTime? ConvertToDateTime(object value)
    {
        if (value != null && DateTime.TryParse(value.ToString(), out DateTime result))
        {
            return result;
        }
        return null;
    }
}