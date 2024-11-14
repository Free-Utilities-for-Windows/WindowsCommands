using System;
using System.Diagnostics;
using WindowsCommands.Logger;

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
                    string logInfo = $"\n-----------------------------------------\n" +
                                     $"Log Name: {log.Log}\n" +
                                     $"Record Count: {log.Entries.Count}";
                    Console.WriteLine(logInfo);
                    StaticFileLogger.LogInformation(logInfo);
                }
                catch (System.Security.SecurityException)
                {
                    string errorMessage = $"Failed to access log: {log.Log}";
                    Console.WriteLine(errorMessage);
                    StaticFileLogger.LogError(errorMessage);
                }
            }
        }
        else
        {
            try
            {
                EventLog log = new EventLog(logName);

                foreach (EventLogEntry entry in log.Entries)
                {
                    string entryInfo = $"\n-----------------------------------------\n" +
                                       $"Time Created: {entry.TimeGenerated}\n" +
                                       $"Level: {entry.EntryType}\n" +
                                       $"Message: {entry.Message}";
                    Console.WriteLine(entryInfo);
                    StaticFileLogger.LogInformation(entryInfo);
                }
            }
            catch (Exception e)
            {
                string errorMessage = $"An error occurred while accessing the event log: {e.Message}";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
            }
        }
    }
}