namespace WindowsCommands;

public static class FilesInformation
{
    public static List<FileData> GetFiles(string path)
    {
        var info = new DirectoryInfo(path);
        var fileInfos = info.GetFileSystemInfos();
        var files = new List<FileData>();

        foreach (var fileInfo in fileInfos)
        {
            var fileData = new FileData
            {
                Name = fileInfo.Name,
                FullName = fileInfo.FullName,
                Type = fileInfo is DirectoryInfo ? "Directory" : "File",
                Size = fileInfo is FileInfo
                    ? ((FileInfo)fileInfo).Length / 1024d / 1024d / 1024d
                    : GetDirectorySize(fileInfo.FullName) / 1024d / 1024d / 1024d,
                CreationTime = fileInfo.CreationTime.ToString("dd/MM/yyyy hh:mm:ss"),
                LastAccessTime = fileInfo.LastAccessTime.ToString("dd/MM/yyyy hh:mm:ss"),
                LastWriteTime = fileInfo.LastWriteTime.ToString("dd/MM/yyyy hh:mm:ss")
            };
            files.Add(fileData);
        }

        return files;
    }

    public static long GetDirectorySize(string path)
    {
        var info = new DirectoryInfo(path);
        long size = 0;

        foreach (var file in info.GetFiles())
        {
            size += file.Length;
        }

        foreach (var directory in info.GetDirectories())
        {
            size += GetDirectorySize(directory.FullName);
        }

        return size;
    }

    public class FileData
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public double Size { get; set; }
        public string CreationTime { get; set; }
        public string LastAccessTime { get; set; }
        public string LastWriteTime { get; set; }
    }
}