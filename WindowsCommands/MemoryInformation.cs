using System;
using System.Diagnostics;
using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class MemoryInformation
{
    public static void GetMemoryInformation()
    {
        try
        {
            var pcRAM = new PerformanceCounter("Memory", "Available MBytes");
            var totalMemory = GetTotalMemoryInMBytes();

            var availablePhysicalMemory = pcRAM.NextValue();
            var usedPhysicalMemory = totalMemory - availablePhysicalMemory;
            var physicalMemoryUsedPercent = usedPhysicalMemory / totalMemory * 100;

            string memoryInfo = $"Total Physical Memory: {totalMemory} MB\n" +
                                $"Used Physical Memory: {usedPhysicalMemory} MB\n" +
                                $"Used Physical Memory: {physicalMemoryUsedPercent} %";
            Console.WriteLine(memoryInfo);
            StaticFileLogger.LogInformation(memoryInfo);

            var processes = Process.GetProcesses();
            var totalWorkingSet = 0L;
            var totalPagedMemorySize = 0L;

            foreach (var process in processes)
            {
                totalWorkingSet += process.WorkingSet64;
                totalPagedMemorySize += process.PagedMemorySize64;
            }

            string processMemoryInfo = $"Total Working Set: {totalWorkingSet / 1024 / 1024} MB\n" +
                                       $"Total Paged Memory Size: {totalPagedMemorySize / 1024 / 1024} MB";
            Console.WriteLine(processMemoryInfo);
            StaticFileLogger.LogInformation(processMemoryInfo);

            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                ulong totalVisibleMemorySize = (ulong)obj["TotalVisibleMemorySize"];
                ulong freePhysicalMemory = (ulong)obj["FreePhysicalMemory"];
                ulong totalVirtualMemorySize = (ulong)obj["TotalVirtualMemorySize"];
                ulong freeVirtualMemory = (ulong)obj["FreeVirtualMemory"];

                ulong memUse = totalVisibleMemorySize - freePhysicalMemory;
                double memUseProc = (double)memUse / totalVisibleMemorySize * 100;

                ulong pageSize = totalVirtualMemorySize - totalVisibleMemorySize;
                ulong pageFree = freeVirtualMemory - freePhysicalMemory;
                ulong pageUse = pageSize - pageFree;
                double pageUseProc = (double)pageUse / pageSize * 100;

                ulong memVirtUse = totalVirtualMemorySize - freeVirtualMemory;
                double memVirtUseProc = (double)memVirtUse / totalVirtualMemorySize * 100;

                string detailedMemoryInfo = $"\nIn GB\n" +
                                            $"MemoryAll: {totalVisibleMemorySize / 1024.0 / 1024.0:0.00} GB\n" +
                                            $"MemoryUse: {memUse / 1024.0 / 1024.0:0.00} GB\n" +
                                            $"MemoryUseProc: {memUseProc:0.00} %\n" +
                                            $"PageSize: {pageSize / 1024.0 / 1024.0:0.00} GB\n" +
                                            $"PageUse: {pageUse / 1024.0 / 1024.0:0.00} GB\n" +
                                            $"PageUseProc: {pageUseProc:0.00} %\n" +
                                            $"MemoryVirtAll: {totalVirtualMemorySize / 1024.0 / 1024.0:0.00} GB\n" +
                                            $"MemoryVirtUse: {memVirtUse / 1024.0 / 1024.0:0.00} GB\n" +
                                            $"MemoryVirtUseProc: {memVirtUseProc:0.00} %";
                Console.WriteLine(detailedMemoryInfo);
                StaticFileLogger.LogInformation(detailedMemoryInfo);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static float GetTotalMemoryInMBytes()
    {
        try
        {
            var cmd = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = cmd.Get();
            foreach (var mo in collection)
            {
                return (float.Parse(mo["TotalPhysicalMemory"].ToString()) / 1024 / 1024);
            }

            return 0;
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
            return 0;
        }
    }
}