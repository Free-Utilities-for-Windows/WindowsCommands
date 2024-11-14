using System.Diagnostics;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class NetworkUtilization
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
                string noInterfacesMessage = "No network interfaces found.";
                Console.WriteLine(noInterfacesMessage);
                StaticFileLogger.LogInformation(noInterfacesMessage);
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

                string utilizationMessage = $"Network Interface: {instanceName}\n";

                if (networkUtilization > CriticalThreshold)
                {
                    utilizationMessage += $"CRITICAL: {networkUtilization}% Network utilization, {averageTransferRate:N0} b/s";
                }
                else if (networkUtilization > WarningThreshold)
                {
                    utilizationMessage += $"WARNING: {networkUtilization}% Network utilization, {averageTransferRate:N0} b/s";
                }
                else
                {
                    utilizationMessage += $"OK: {networkUtilization}% Network utilization, {averageTransferRate:N0} b/s";
                }

                Console.WriteLine(utilizationMessage);
                StaticFileLogger.LogInformation(utilizationMessage);
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"An error occurred: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}