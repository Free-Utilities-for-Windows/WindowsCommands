using System.Diagnostics;

namespace WindowsCommands;

public static class MemoryInformation
{
    public static void GetMemoryInformation()
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
    }

    private static float GetTotalMemoryInMBytes()
    {
        var cmd = new System.Management.ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
        System.Management.ManagementObjectCollection collection = cmd.Get();

        foreach (var mo in collection)
        {
            return (float.Parse(mo["TotalPhysicalMemory"].ToString()) / 1024 / 1024);
        }
        return 0;
    }
}