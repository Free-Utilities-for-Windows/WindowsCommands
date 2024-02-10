using System.Management;

namespace WindowsCommands;

public static class CPUInformation
{
    public static void GetCPUInformation()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");

            var values1 = GetValues(searcher);
            System.Threading.Thread.Sleep(1000);
            var values2 = GetValues(searcher);

            foreach (var key in values1.Keys)
            {
                Console.WriteLine("Name: {0}", key);
                Console.WriteLine("ProcessorTime: {0} %", values2[key]["PercentProcessorTime"] - values1[key]["PercentProcessorTime"]);
                Console.WriteLine("PrivilegedTime: {0} %", values2[key]["PercentPrivilegedTime"] - values1[key]["PercentPrivilegedTime"]);
                Console.WriteLine("UserTime: {0} %", values2[key]["PercentUserTime"] - values1[key]["PercentUserTime"]);
                Console.WriteLine("InterruptTime: {0} %", values2[key]["PercentInterruptTime"] - values1[key]["PercentInterruptTime"]);
                Console.WriteLine("IdleTime: {0} %", values2[key]["PercentIdleTime"] - values1[key]["PercentIdleTime"]);
                Console.WriteLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    private static Dictionary<string, Dictionary<string, long>> GetValues(ManagementObjectSearcher searcher)
    {
        var values = new Dictionary<string, Dictionary<string, long>>();

        try
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                var name = (string)obj["Name"];
                values[name] = new Dictionary<string, long>
                {
                    ["PercentProcessorTime"] = ConvertToLong((ulong)obj["PercentProcessorTime"]),
                    ["PercentPrivilegedTime"] = ConvertToLong((ulong)obj["PercentPrivilegedTime"]),
                    ["PercentUserTime"] = ConvertToLong((ulong)obj["PercentUserTime"]),
                    ["PercentInterruptTime"] = ConvertToLong((ulong)obj["PercentInterruptTime"]),
                    ["PercentIdleTime"] = ConvertToLong((ulong)obj["PercentIdleTime"]),
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while getting values: " + e.Message);
        }

        return values;
    }

    private static long ConvertToLong(ulong value)
    {
        try
        {
            if (value <= long.MaxValue)
            {
                return (long)value;
            }
            else
            {
                throw new OverflowException("Value is too large to fit into a long");
            }
        }
        catch (OverflowException e)
        {
            Console.WriteLine("An error occurred while converting value: " + e.Message);
            return 0;
        }
    }
}