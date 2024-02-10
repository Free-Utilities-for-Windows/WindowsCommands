using System;
using System.Diagnostics;
using System.Management;

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

            Console.WriteLine($"Total Physical Memory: {totalMemory} MB");
            Console.WriteLine($"Used Physical Memory: {usedPhysicalMemory} MB");
            Console.WriteLine($"Used Physical Memory: {physicalMemoryUsedPercent} %");

            var processes = Process.GetProcesses();
            var totalWorkingSet = 0L;
            var totalPagedMemorySize = 0L;

            foreach (var process in processes)
            {
                totalWorkingSet += process.WorkingSet64;
                totalPagedMemorySize += process.PagedMemorySize64;
            }

            Console.WriteLine($"Total Working Set: {totalWorkingSet / 1024 / 1024} MB");
            Console.WriteLine($"Total Paged Memory Size: {totalPagedMemorySize / 1024 / 1024} MB");

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

                Console.WriteLine($"");
                Console.WriteLine($"In GB");
                Console.WriteLine($"MemoryAll: {totalVisibleMemorySize / 1024.0 / 1024.0:0.00} GB");
                Console.WriteLine($"MemoryUse: {memUse / 1024.0 / 1024.0:0.00} GB");
                Console.WriteLine($"MemoryUseProc: {memUseProc:0.00} %");
                Console.WriteLine($"PageSize: {pageSize / 1024.0 / 1024.0:0.00} GB");
                Console.WriteLine($"PageUse: {pageUse / 1024.0 / 1024.0:0.00} GB");
                Console.WriteLine($"PageUseProc: {pageUseProc:0.00} %");
                Console.WriteLine($"MemoryVirtAll: {totalVirtualMemorySize / 1024.0 / 1024.0:0.00} GB");
                Console.WriteLine($"MemoryVirtUse: {memVirtUse / 1024.0 / 1024.0:0.00} GB");
                Console.WriteLine($"MemoryVirtUseProc: {memVirtUseProc:0.00} %");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    private static float GetTotalMemoryInMBytes()
    {
        try
        {
            var cmd = new System.Management.ManagementObjectSearcher(
                "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            System.Management.ManagementObjectCollection collection = cmd.Get();
            foreach (var mo in collection)
            {
                return (float.Parse(mo["TotalPhysicalMemory"].ToString()) / 1024 / 1024);
            }

            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            return 0;
        }
    }
}