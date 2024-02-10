using System;
using System.Management;

namespace WindowsCommands;

public static class NetworkInterfaceStats
{
    public static void GetNetworkInterfaceStats(bool current)
    {
        try
        {
            string className = current ? "Win32_PerfFormattedData_Tcpip_NetworkInterface" : "Win32_PerfRawData_Tcpip_NetworkInterface";
            var searcher = new ManagementObjectSearcher($"SELECT * FROM {className}");

            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine("Name: " + obj["Name"]);
                Console.WriteLine("Total: " + ConvertBytes(obj["BytesTotalPersec"], current));
                Console.WriteLine("Received: " + ConvertBytes(obj["BytesReceivedPersec"], current));
                Console.WriteLine("Sent: " + ConvertBytes(obj["BytesSentPersec"], current));
                Console.WriteLine("PacketsPersec: " + obj["PacketsPersec"]);
                Console.WriteLine("PacketsReceivedPersec: " + obj["PacketsReceivedPersec"]);
                Console.WriteLine("PacketsReceivedUnicastPersec: " + obj["PacketsReceivedUnicastPersec"]);
                Console.WriteLine("PacketsReceivedNonUnicastPersec: " + obj["PacketsReceivedNonUnicastPersec"]);
                Console.WriteLine("PacketsReceivedDiscarded: " + obj["PacketsReceivedDiscarded"]);
                Console.WriteLine("PacketsReceivedErrors: " + obj["PacketsReceivedErrors"]);
                Console.WriteLine("PacketsSentPersec: " + obj["PacketsSentPersec"]);
                Console.WriteLine("PacketsSentUnicastPersec: " + obj["PacketsSentUnicastPersec"]);
                Console.WriteLine("PacketsSentNonUnicastPersec: " + obj["PacketsSentNonUnicastPersec"]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    private static string ConvertBytes(object bytes, bool current)
    {
        try
        {
            long bytesLong = Convert.ToInt64(bytes);
            if (current)
            {
                return (bytesLong / 1e6).ToString("0.000 MByte/Sec");
            }
            else
            {
                return (bytesLong / 1e9).ToString("0.00 GByte");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while converting bytes: " + e.Message);
            return "Error";
        }
    }
}