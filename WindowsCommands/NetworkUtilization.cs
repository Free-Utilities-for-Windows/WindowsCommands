using System;
using System.Diagnostics;
using System.Threading;

namespace WindowsCommands;

public class NetworkUtilization
{
    private const float WarningThreshold = 25;
    private const float CriticalThreshold = 50;
    private const int SampleCount = 5;
    private const int SampleIntervalMilliseconds = 1000;

    public static void MonitorNetworkUtilization()
    {
        try
        {
            var category = new PerformanceCounterCategory("Network Interface");
            
            var instanceNames = category.GetInstanceNames();
            if (instanceNames.Length == 0)
            {
                Console.WriteLine("No network interfaces found.");
                return;
            }

            foreach (var instanceName in instanceNames)
            {
                var networkCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instanceName);
                
                networkCounter.NextValue();
                
                Thread.Sleep(SampleIntervalMilliseconds);

                float totalTransferRate = 0;

                for (int i = 0; i < SampleCount; i++)
                {
                    float transferRate = networkCounter.NextValue() * 8;
                    totalTransferRate += transferRate;
                    
                    Thread.Sleep(SampleIntervalMilliseconds);
                }
                
                float averageTransferRate = totalTransferRate / SampleCount;
                
                float networkUtilization = MathF.Round(averageTransferRate / 1000000000 * 100, 2);

                Console.WriteLine($"Network Interface: {instanceName}");

                if (networkUtilization > CriticalThreshold)
                {
                    Console.WriteLine($"CRITICAL: {networkUtilization}% Network utilization, {averageTransferRate:N0} b/s");
                    // Environment.Exit(2);
                }
                else if (networkUtilization > WarningThreshold)
                {
                    Console.WriteLine($"WARNING: {networkUtilization}% Network utilization, {averageTransferRate:N0} b/s");
                    // Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine($"OK: {networkUtilization}% Network utilization, {averageTransferRate:N0} b/s");
                    // Environment.Exit(0);
                }

                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // Environment.Exit(3);
        }
    }
}