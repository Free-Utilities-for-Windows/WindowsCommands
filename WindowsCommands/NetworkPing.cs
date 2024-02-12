using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WindowsCommands;


public static class NetworkPing
{
    public static void PingNetwork(string network, int timeout = 100)
    {
        try
        {
            var baseIp = network.Substring(0, network.LastIndexOf('.') + 1);
            var pinger = new Ping();

            for (int i = 1; i < 255; i++)
            {
                if (Console.KeyAvailable)
                {
                    Console.WriteLine("Execution interrupted by user.");
                    Environment.Exit(0);
                }
                
                var ip = baseIp + i;
                PingReply reply;

                try
                {
                    reply = pinger.Send(ip, timeout);
                }
                catch (PingException ex)
                {
                    Console.WriteLine($"Error pinging address: {ip}. Exception: {ex.Message}");
                    continue;
                }

                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Address: {ip}, Status: Success");
                }
                else
                {
                    Console.WriteLine($"Address: {ip}, Status: {reply.Status}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}