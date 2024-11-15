﻿using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class Battery
{
    public static void GetBatteryInfo()
    {
        try
        {
            ManagementObjectSearcher searcher =
                new(@"root\CIMV2", "SELECT * FROM Win32_Battery");

            foreach (ManagementObject item in searcher.Get())
            {
                string designVoltage = $"Design voltage: {item["DesignVoltage"]} mV";
                string estimatedCharge = $"Estimated charge remaining: {item["EstimatedChargeRemaining"]}%";
                string batteryStatus = $"Battery status: {(BatteryStatus)(ushort)item["BatteryStatus"]}";

                Console.WriteLine(designVoltage);
                Console.WriteLine(estimatedCharge);
                Console.WriteLine(batteryStatus);

                StaticFileLogger.LogInformation(designVoltage);
                StaticFileLogger.LogInformation(estimatedCharge);
                StaticFileLogger.LogInformation(batteryStatus);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public enum BatteryStatus
    {
        Discharging = 1,
        Unknown = 2,
        FullyCharged = 3,
        Low = 4,
        Critical = 5,
        Charging = 6,
        ChargingAndHigh = 7,
        ChargingAndLow = 8,
        ChargingAndCritical = 9,
        Undefined = 10,
        PartiallyCharged = 11,
    }
}