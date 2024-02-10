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
                Console.WriteLine("Description: {0}", obj["Description"]);
                Console.WriteLine("DHCPEnabled: {0}", obj["DHCPEnabled"]);
                Console.WriteLine("DHCPLeaseObtained: {0}", ConvertToDateTime(obj["DHCPLeaseObtained"]));
                Console.WriteLine("DHCPLeaseExpires: {0}", ConvertToDateTime(obj["DHCPLeaseExpires"]));
                Console.WriteLine("DHCPServer: {0}", obj["DHCPServer"]);
                Console.WriteLine("IPAddress: {0}", String.Join(", ", (string[])obj["IPAddress"]));
                Console.WriteLine("DefaultIPGateway: {0}", String.Join(", ", (string[])obj["DefaultIPGateway"]));
                Console.WriteLine("IPSubnet: {0}", String.Join(", ", (string[])obj["IPSubnet"]));
                Console.WriteLine("MACAddress: {0}", obj["MACAddress"]);
                Console.WriteLine("\n-----------------------------------------");
            }
        }
        catch (ManagementException e)
        {
            Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
        }
        catch (System.Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
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
        catch (System.Exception e)
        {
            Console.WriteLine("An error occurred while converting time: " + e.Message);
        }
        return DateTime.MinValue;
    }
}