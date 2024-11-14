using System;
using System.Management;
using WindowsCommands.Logger;

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
                string statsInfo = $"Name: {obj["Name"]}\n" +
                                   $"Total: {ConvertBytes(obj["BytesTotalPersec"], current)}\n" +
                                   $"Received: {ConvertBytes(obj["BytesReceivedPersec"], current)}\n" +
                                   $"Sent: {ConvertBytes(obj["BytesSentPersec"], current)}\n" +
                                   $"PacketsPersec: {obj["PacketsPersec"]}\n" +
                                   $"PacketsReceivedPersec: {obj["PacketsReceivedPersec"]}\n" +
                                   $"PacketsReceivedUnicastPersec: {obj["PacketsReceivedUnicastPersec"]}\n" +
                                   $"PacketsReceivedNonUnicastPersec: {obj["PacketsReceivedNonUnicastPersec"]}\n" +
                                   $"PacketsReceivedDiscarded: {obj["PacketsReceivedDiscarded"]}\n" +
                                   $"PacketsReceivedErrors: {obj["PacketsReceivedErrors"]}\n" +
                                   $"PacketsSentPersec: {obj["PacketsSentPersec"]}\n" +
                                   $"PacketsSentUnicastPersec: {obj["PacketsSentUnicastPersec"]}\n" +
                                   $"PacketsSentNonUnicastPersec: {obj["PacketsSentNonUnicastPersec"]}\n";
                Console.WriteLine(statsInfo);
                StaticFileLogger.LogInformation(statsInfo);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
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
            string errorMessage = "An error occurred while converting bytes: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
            return "Error";
        }
    }
}