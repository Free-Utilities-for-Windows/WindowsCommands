using System;
using System.Management;

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
                Console.WriteLine("Description: " + obj["Description"]);
                Console.WriteLine("IPAddress: " + (obj["IPAddress"] != null ? String.Join(", ", (string[])obj["IPAddress"]) : "N/A"));
                Console.WriteLine("GatewayDefault: " + (obj["DefaultIPGateway"] != null ? String.Join(", ", (string[])obj["DefaultIPGateway"]) : "N/A"));
                Console.WriteLine("Subnet: " + (obj["IPSubnet"] != null ? String.Join(", ", (string[])obj["IPSubnet"]) : "N/A"));
                Console.WriteLine("DNSServer: " + (obj["DNSServerSearchOrder"] != null ? String.Join(", ", (string[])obj["DNSServerSearchOrder"]) : "N/A"));
                Console.WriteLine("MACAddress: " + obj["MACAddress"]);
                Console.WriteLine("DHCPEnabled: " + obj["DHCPEnabled"]);
                Console.WriteLine("DHCPServer: " + obj["DHCPServer"]);
                Console.WriteLine("DHCPLeaseObtained: " + ConvertToDateTime(obj["DHCPLeaseObtained"]));
                Console.WriteLine("DHCPLeaseExpires: " + ConvertToDateTime(obj["DHCPLeaseExpires"]));
                Console.WriteLine("\n-----------------------------------------");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
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