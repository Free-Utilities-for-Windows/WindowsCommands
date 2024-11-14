using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class MemorySlotInfo
{
    public static void PrintMemorySlots()
    {
        try
        {
            var memorySlots = GetMemorySlots();

            foreach (var slot in memorySlots)
            {
                string slotInfo = $"Tag: {slot.Tag}\n" +
                                  $"Model: {slot.Model}\n" +
                                  $"Size: {slot.Size}\n" +
                                  $"Device: {slot.Device}\n" +
                                  $"Bank: {slot.Bank}\n";
                Console.WriteLine(slotInfo);
                StaticFileLogger.LogInformation(slotInfo);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while printing memory slots: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public static List<MemorySlot> GetMemorySlots()
    {
        var memorySlots = new List<MemorySlot>();

        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                var memorySlot = new MemorySlot
                {
                    Tag = queryObj["Tag"].ToString(),
                    Model = $"{queryObj["ConfiguredClockSpeed"]} Mhz {queryObj["Manufacturer"]} {queryObj["PartNumber"]}",
                    Size = $"{Convert.ToInt64(queryObj["Capacity"]) / (1024 * 1024)} Mb",
                    Device = queryObj["DeviceLocator"].ToString(),
                    Bank = queryObj["BankLabel"].ToString()
                };

                memorySlots.Add(memorySlot);
            }

            StaticFileLogger.LogInformation("Successfully retrieved memory slots information.");
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while getting memory slots: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }

        return memorySlots;
    }

    public class MemorySlot
    {
        public string Tag { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string Device { get; set; }
        public string Bank { get; set; }
    }
}