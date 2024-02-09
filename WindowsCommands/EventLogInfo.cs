using System;
using System.Diagnostics;

namespace WindowsCommands;

public static class EventLogInfo
{
    public static void GetEvent(string logName)
    {
        if (string.IsNullOrEmpty(logName))
        {
            foreach (EventLog log in EventLog.GetEventLogs())
            {
                try
                {
                    Console.WriteLine("\n-----------------------------------------");
                    Console.WriteLine("Log Name: {0}", log.Log);
                    Console.WriteLine("Record Count: {0}", log.Entries.Count);
                }
                catch (System.Security.SecurityException)
                {
                    Console.WriteLine("Failed to access log: {0}", log.Log);
                }
            }
        }
        else
        {
            EventLog log = new EventLog(logName);

            foreach (EventLogEntry entry in log.Entries)
            {
                Console.WriteLine("\n-----------------------------------------");
                Console.WriteLine("Time Created: {0}", entry.TimeGenerated);
                Console.WriteLine("Level: {0}", entry.EntryType);
                Console.WriteLine("Message: {0}", entry.Message);
            }
        }
    }
}