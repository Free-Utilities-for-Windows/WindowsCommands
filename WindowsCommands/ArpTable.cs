using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace WindowsCommands;

public static class ArpTable
{
    public static void GetArpTable(string proxy = null, string search = null)
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

                Console.WriteLine($"IP: {ip}, MAC: {mac}, Type: {type}");
            }
        }
    }
}