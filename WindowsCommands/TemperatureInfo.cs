using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class TemperatureInfo
{
    public static void GetTemperature()
    {
        try
        {
            var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                UInt32 tempKelvin = Convert.ToUInt32(queryObj["CurrentTemperature"]);
                double tempCelsius = tempKelvin / 10.0 - 273.15;
                string tempMessage = $"Temperature: {tempCelsius}°C";
                Console.WriteLine(tempMessage);
                StaticFileLogger.LogInformation(tempMessage);
                return;
            }
        }
        catch (ManagementException e)
        {
            string errorMessage = "Cannot get temperature: An error occurred while querying for WMI data: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
        catch (Exception e)
        {
            string errorMessage = "Cannot get temperature: An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }

        try
        {
            var searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_TemperatureProbe");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["CurrentReading"] != null)
                {
                    UInt32 tempKelvin = Convert.ToUInt32(queryObj["CurrentReading"]);
                    double tempCelsius = tempKelvin / 10.0 - 273.15;
                    string tempMessage = $"Temperature (Celsius): {tempCelsius}";
                    Console.WriteLine(tempMessage);
                    StaticFileLogger.LogInformation(tempMessage);
                    return;
                }
            }

            searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_ThermalZone");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["CurrentTemperature"] != null)
                {
                    UInt32 tempKelvin = Convert.ToUInt32(queryObj["CurrentTemperature"]);
                    double tempCelsius = tempKelvin / 10.0 - 273.15;
                    string tempMessage = $"Temperature (Celsius): {tempCelsius}";
                    Console.WriteLine(tempMessage);
                    StaticFileLogger.LogInformation(tempMessage);
                    return;
                }
            }
        }
        catch (ManagementException e)
        {
            string errorMessage = "Cannot get temperature: An error occurred while querying for WMI data: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
        catch (Exception e)
        {
            string errorMessage = "Cannot get temperature: An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }

        string unsupportedMessage = "Cannot get temperature: Unsupported on this system";
        Console.WriteLine(unsupportedMessage);
        StaticFileLogger.LogError(unsupportedMessage);
    }
}