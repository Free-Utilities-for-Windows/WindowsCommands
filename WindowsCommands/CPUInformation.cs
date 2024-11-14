using System.Management;
using System.Runtime.InteropServices;

namespace WindowsCommands;

public static class CPUInformation
{
    public static void GetCPUInformation()
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");

                var values = GetValues(searcher);

                foreach (var key in values.Keys)
                {
                    Console.WriteLine("Name: {0}", key);
                    Console.WriteLine("ProcessorTime: {0} %", values[key]["PercentProcessorTime"]);
                    Console.WriteLine("PrivilegedTime: {0} %", values[key]["PercentPrivilegedTime"]);
                    Console.WriteLine("UserTime: {0} %", values[key]["PercentUserTime"]);
                    Console.WriteLine("InterruptTime: {0} %", values[key]["PercentInterruptTime"]);
                    Console.WriteLine("IdleTime: {0} %", values[key]["PercentIdleTime"]);
                    Console.WriteLine();
                }

                Console.WriteLine("Processors Count: {0}", GetProcessorsCount());
                Console.WriteLine("Logical Processors: {0}", GetLogicalProcessors());
                Console.WriteLine("Number of Cores: {0}", GetNumberOfCores());
                Console.WriteLine("Physical Processors: {0}", GetPhysicalProcessors());
                Console.WriteLine("Excluded Processors: {0}", GetExcludedProcessors());
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            System.Threading.Thread.Sleep(1000);
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

    public static int GetProcessorsCount()
    {
        return Environment.ProcessorCount;
    }

    public static int GetLogicalProcessors()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }

        const string wmiQuery = "Select * from Win32_ComputerSystem";
        foreach (var item in new ManagementObjectSearcher(wmiQuery).Get())
        {
            return int.Parse(item["NumberOfLogicalProcessors"].ToString());
        }

        return 0;
    }

    public static int GetNumberOfCores()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }

        var coreCount = 0;
        const string wmiQuery = "Select * from Win32_Processor";
        foreach (var item in new ManagementObjectSearcher(wmiQuery).Get())
        {
            coreCount += int.Parse(item["NumberOfCores"].ToString());
        }

        return coreCount;
    }

    public static int GetPhysicalProcessors()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }

        const string wmiQuery = "Select * from Win32_ComputerSystem";
        foreach (var item in new ManagementObjectSearcher(wmiQuery).Get())
        {
            return int.Parse(item["NumberOfProcessors"].ToString());
        }

        return 0;
    }

    public static int GetExcludedProcessors()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }

        var deviceCount = 0;
        var deviceList = IntPtr.Zero;
        var processorGuid = new Guid("{50127dc3-0f36-415e-a6cc-4cb3be910b65}");

        try
        {
            deviceList = SetupDiGetClassDevs(ref processorGuid, "ACPI", IntPtr.Zero, (int)DIGCF.PRESENT);
            for (var deviceNumber = 0; ; deviceNumber++)
            {
                var deviceInfo = new SP_DEVINFO_DATA();
                deviceInfo.cbSize = Marshal.SizeOf(deviceInfo);

                if (!SetupDiEnumDeviceInfo(deviceList, deviceNumber, ref deviceInfo))
                {
                    deviceCount = deviceNumber;
                    break;
                }
            }
        }
        finally
        {
            if (deviceList != IntPtr.Zero) { SetupDiDestroyDeviceInfoList(deviceList); }
        }

        return deviceCount;
    }

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid,
        [MarshalAs(UnmanagedType.LPStr)] string enumerator,
        IntPtr hwndParent,
        int Flags);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern int SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet,
        int MemberIndex,
        ref SP_DEVINFO_DATA DeviceInterfaceData);

    [StructLayout(LayoutKind.Sequential)]
    private struct SP_DEVINFO_DATA
    {
        public int cbSize;
        public Guid ClassGuid;
        public uint DevInst;
        public IntPtr Reserved;
    }

    private enum DIGCF
    {
        DEFAULT = 0x1,
        PRESENT = 0x2,
        ALLCLASSES = 0x4,
        PROFILE = 0x8,
        DEVICEINTERFACE = 0x10,
    }
}