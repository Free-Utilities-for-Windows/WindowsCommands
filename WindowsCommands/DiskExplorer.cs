using System.Management;

namespace WindowsCommands;

public static class DiskExplorer
{
    public static void GetDrives()
    {
        const string query = "Select DeviceID, Caption, InterfaceType, MediaType, Size FROM Win32_DiskDrive";
        ManagementObjectSearcher searcher = new(@"root\CIMV2", query);

        foreach (ManagementObject moDrive in searcher.Get())
        {
            string deviceID = moDrive["DeviceID"] as string ?? "";
            string caption = moDrive["Caption"] as string ?? "";
            ulong size = moDrive["Size"] as ulong? ?? 0;
            string mediaType = moDrive["MediaType"] as string ?? "";
            string interfaceType = moDrive["InterfaceType"] as string ?? "";
            var drive = new PhysicalDrive(deviceID, caption, size, mediaType, interfaceType);
            Console.WriteLine(
                $"ID: {drive.ID}, Name: {drive.Name}, Size: {drive.SizeGbText} GB, MediaType: {drive.MediaType}, InterfaceType: {drive.InterfaceType}");
        }
    }

    public static void GetLogicalDrives(string deviceId)
    {
        string partitionQuery =
            $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceId}'}}  WHERE ASSOCCLASS = Win32_DiskDriveToDiskPartition";

        ManagementObjectSearcher searcher = new(@"root\CIMV2", partitionQuery);
        ManagementObjectCollection partitions = searcher.Get();
        foreach (ManagementObject moPartition in partitions)
        {
            string partitionID = moPartition["DeviceID"] as string ?? "";
            ulong size = moPartition["Size"] as ulong? ?? 0;
            bool isBootable = moPartition["Bootable"] as bool? ?? false;

            string ldQuery =
                $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionID}'}} WHERE ASSOCCLASS = Win32_LogicalDiskToPartition";
            ManagementObjectSearcher ldSearcher = new(@"root\CIMV2", ldQuery);

            ManagementObjectCollection logicalDisks = ldSearcher.Get();

            List<string> roots = new();
            foreach (ManagementObject logicalDisk in logicalDisks)
                roots.Add(logicalDisk["DeviceID"] as string ?? "");
            string directoryRoot = string.Join(',', roots);

            var logicalDrive = new LogicalDrive(directoryRoot, partitionID, size, isBootable);
            Console.WriteLine(
                $"DirectoryRoot: {logicalDrive.DirectoryRoot}, PartitionID: {logicalDrive.PartitionID}, Size: {logicalDrive.SizeGbText} GB, IsBootable: {logicalDrive.IsBootable}");
        }
    }

    public class LogicalDrive
    {
        public static ulong OneGb = 1024 * 1024 * 1024;

        public string DirectoryRoot { get; }
        public string PartitionID { get; }
        public ulong PartitionSizeBytes { get; }
        public bool IsBootable { get; }

        public string SizeGbText => ((double)PartitionSizeBytes / OneGb).ToString("###,###,###.##");

        public LogicalDrive(string directoryRoot, string partitionID, ulong partitionSizeBytes, bool isBootable)
        {
            DirectoryRoot = directoryRoot;
            PartitionID = partitionID;
            PartitionSizeBytes = partitionSizeBytes;
            IsBootable = isBootable;
        }
    }

    public class PhysicalDrive
    {
        public static ulong OneGb = 1024 * 1024 * 1024;

        public string ID { get; }
        public string Name { get; }
        public string InterfaceType { get; }
        public string MediaType { get; }
        public ulong SizeBytes { get; }

        public string SizeGbText => ((double)SizeBytes / OneGb).ToString("###,###,###.##");

        public PhysicalDrive(string id, string name, ulong sizeBytes, string mediaType, string interfaceType)
        {
            ID = id;
            Name = name;
            InterfaceType = interfaceType;
            MediaType = mediaType;
            SizeBytes = sizeBytes;
        }
    }
}