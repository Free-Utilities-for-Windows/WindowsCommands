using System.Management;

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
                Console.WriteLine($"Design voltage: {item["DesignVoltage"]} mV");
                Console.WriteLine(
                    $"Estimated charge remaining: {item["EstimatedChargeRemaining"]}%");
                Console.WriteLine(
                    $"Battery status: {(BatteryStatus)(ushort)item["BatteryStatus"]}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
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