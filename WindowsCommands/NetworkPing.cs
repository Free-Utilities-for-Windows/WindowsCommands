using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WindowsCommands.Logger;

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
                    string interruptMessage = "Execution interrupted by user.";
                    Console.WriteLine(interruptMessage);
                    StaticFileLogger.LogInformation(interruptMessage);
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
                    string errorMessage = $"Error pinging address: {ip}. Exception: {ex.Message}";
                    Console.WriteLine(errorMessage);
                    StaticFileLogger.LogError(errorMessage);
                    continue;
                }

                string pingResult = reply.Status == IPStatus.Success
                    ? $"Address: {ip}, Status: Success"
                    : $"Address: {ip}, Status: {reply.Status}";
                Console.WriteLine(pingResult);
                StaticFileLogger.LogInformation(pingResult);
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"An error occurred: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}