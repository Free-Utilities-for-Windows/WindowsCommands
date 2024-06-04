using System.Management;

namespace WindowsCommands;

public static class MemorySlotInfo
{
    public static void PrintMemorySlots()
    {
        var memorySlots = GetMemorySlots();

        foreach (var slot in memorySlots)
        {
            Console.WriteLine($"Tag: {slot.Tag}");
            Console.WriteLine($"Model: {slot.Model}");
            Console.WriteLine($"Size: {slot.Size}");
            Console.WriteLine($"Device: {slot.Device}");
            Console.WriteLine($"Bank: {slot.Bank}");
            Console.WriteLine();
        }
    }
    
    public static List<MemorySlot> GetMemorySlots()
    {
        var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
        var memorySlots = new List<MemorySlot>();

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