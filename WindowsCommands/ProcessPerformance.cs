using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace WindowsCommands;

public class ProcessPerformance
{
    public string Name { get; set; }
    public double ProcTime { get; set; }
    public long IOps { get; set; }
    public double IObsRead { get; set; }
    public double IObsWrite { get; set; }
    public TimeSpan RunTime { get; set; }
    public TimeSpan TotalTime { get; set; }
    public TimeSpan UserTime { get; set; }
    public TimeSpan PrivTime { get; set; }
    public double WorkingSet { get; set; }
    public double PeakWorkingSet { get; set; }
    public double PageMemory { get; set; }
    public int Threads { get; set; }
    public int Handles { get; set; }
}

public class ProcessPerformanceCollector
{
    public List<ProcessPerformance> GetProcessPerformance(string processName = null)
    {
        var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
        var processPerformances = searcher.Get().Cast<ManagementObject>().ToList();

        var processes = string.IsNullOrEmpty(processName)
            ? Process.GetProcesses().Where(p => p.ProcessName != "Idle")
            : Process.GetProcessesByName(processName);

        var performances = new List<ProcessPerformance>();

        foreach (var process in processes)
        {
            var processPerformance =
                processPerformances.FirstOrDefault(pp => pp["IDProcess"].ToString() == process.Id.ToString());

            if (processPerformance != null)
            {
                var performance = new ProcessPerformance
                {
                    Name = process.ProcessName,
                    ProcTime = Convert.ToDouble(processPerformance["PercentProcessorTime"]),
                    IOps = Convert.ToInt64(processPerformance["IODataOperationsPersec"]),
                    IObsRead = Convert.ToDouble(processPerformance["IOReadBytesPersec"]) / (1024.0 * 1024.0),
                    IObsWrite = Convert.ToDouble(processPerformance["IOWriteBytesPersec"]) / (1024.0 * 1024.0),
                    TotalTime = process.TotalProcessorTime,
                    UserTime = process.UserProcessorTime,
                    PrivTime = process.PrivilegedProcessorTime,
                    WorkingSet = process.WorkingSet64 / (1024.0 * 1024.0),
                    PeakWorkingSet = process.PeakWorkingSet64 / (1024.0 * 1024.0),
                    PageMemory = process.PagedMemorySize64 / (1024.0 * 1024.0),
                    Threads = process.Threads.Count,
                    Handles = process.HandleCount
                };

                try
                {
                    performance.RunTime = DateTime.Now - process.StartTime;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    performance.RunTime = TimeSpan.Zero;
                }

                performances.Add(performance);
            }
        }

        return performances.OrderByDescending(p => p.ProcTime).ThenByDescending(p => p.IOps)
            .ThenByDescending(p => p.TotalTime.TotalMilliseconds).ToList();
    }
}