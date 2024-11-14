using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class ArpTable
{
    public static void GetArpTable(string proxy = null, string search = null)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "arp",
                Arguments = "-a",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    string ip = parts[0];
                    string mac = parts[1];
                    string type = parts[2];

                    if (search != null && !ip.Contains(search))
                    {
                        continue;
                    }

                    string logMessage = $"IP: {ip}, MAC: {mac}, Type: {type}";
                    Console.WriteLine(logMessage);
                    StaticFileLogger.LogInformation(logMessage);
                }
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