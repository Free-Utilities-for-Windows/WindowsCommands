using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WindowsCommands;

public static class UserSessionQuery
{
    public static async Task GetQuery(string srv = "localhost", string user = "*")
    {
        try
        {
            var users = new List<UserSession>();

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "query",
                Arguments = $"user /server:{srv}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1);

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 6 || parts.Length == 7)
                    {
                        var status = parts[2].Length == 4 ? "Disconnect" : "Active";
                        var session = parts.Length == 7 ? parts[1] : null;
                        var id = parts.Length == 7 ? parts[2] : parts[1];
                        var idleTime = parts.Length == 7 ? parts[4] : parts[3];
                        var logonTime = parts.Length == 7 ? parts[5] + " " + parts[6] : parts[4] + " " + parts[5];

                        users.Add(new UserSession
                        {
                            User = parts[0],
                            Session = session,
                            ID = id,
                            Status = status,
                            IdleTime = idleTime,
                            LogonTime = logonTime
                        });
                    }
                }
            }

            foreach (var userSession in users)
            {
                Console.WriteLine(
                    $"User: {userSession.User}, Session: {userSession.Session}, ID: {userSession.ID}, Status: {userSession.Status}, IdleTime: {userSession.IdleTime}, LogonTime: {userSession.LogonTime}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public class UserSession
    {
        public string User { get; set; }
        public string Session { get; set; }
        public string ID { get; set; }
        public string Status { get; set; }
        public string IdleTime { get; set; }
        public string LogonTime { get; set; }
    }
}