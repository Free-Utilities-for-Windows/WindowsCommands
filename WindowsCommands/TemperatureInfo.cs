using System.Management;

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
                Console.WriteLine("Temperature: {0}°C", tempCelsius);
                return;
            }
        }
        catch (ManagementException e)
        {
            Console.WriteLine("Cannot get temperature: An error occurred while querying for WMI data: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot get temperature: An error occurred: " + e.Message);
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
                    Console.WriteLine("Temperature (Celsius): " + tempCelsius);
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
                    Console.WriteLine("Temperature (Celsius): " + tempCelsius);
                    return;
                }
            }
        }
        catch (ManagementException e)
        {
            Console.WriteLine("Cannot get temperature: An error occurred while querying for WMI data: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot get temperature: An error occurred: " + e.Message);
        }

        Console.WriteLine("Cannot get temperature: Unsupported on this system");
    }
}