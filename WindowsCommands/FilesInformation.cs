using WindowsCommands.Logger;

namespace WindowsCommands;

public static class FilesInformation
{
    public static List<FileData> GetFiles(string path)
    {
        var files = new List<FileData>();
        try
        {
            AddFilesFromDirectory(files, path);
            StaticFileLogger.LogInformation($"Successfully retrieved files from directory: {path}");
        }
        catch (Exception e)
        {
            string errorMessage = $"An error occurred while getting files from directory {path}: {e.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
        return files;
    }

    private static void AddFilesFromDirectory(List<FileData> files, string path)
    {
        var info = new DirectoryInfo(path);

        foreach (var fileInfo in info.GetFileSystemInfos())
        {
            if (fileInfo is DirectoryInfo directoryInfo)
            {
                AddFilesFromDirectory(files, directoryInfo.FullName);

                var fileData = new FileData
                {
                    Name = directoryInfo.Name,
                    FullName = directoryInfo.FullName,
                    Type = "Directory",
                    Size = GetDirectorySize(directoryInfo.FullName) / 1024d / 1024d / 1024d,
                    CreationTime = directoryInfo.CreationTime.ToString("dd/MM/yyyy hh:mm:ss"),
                    LastAccessTime = directoryInfo.LastAccessTime.ToString("dd/MM/yyyy hh:mm:ss"),
                    LastWriteTime = directoryInfo.LastWriteTime.ToString("dd/MM/yyyy hh:mm:ss")
                };
                files.Add(fileData);
                StaticFileLogger.LogInformation($"Added directory: {fileData.FullName}");
            }
            else
            {
                var fileData = new FileData
                {
                    Name = fileInfo.Name,
                    FullName = fileInfo.FullName,
                    Type = "File",
                    Size = ((FileInfo)fileInfo).Length / 1024d / 1024d / 1024d,
                    CreationTime = fileInfo.CreationTime.ToString("dd/MM/yyyy hh:mm:ss"),
                    LastAccessTime = fileInfo.LastAccessTime.ToString("dd/MM/yyyy hh:mm:ss"),
                    LastWriteTime = fileInfo.LastWriteTime.ToString("dd/MM/yyyy hh:mm:ss")
                };
                files.Add(fileData);
                StaticFileLogger.LogInformation($"Added file: {fileData.FullName}");
            }
        }
    }

    public static long GetDirectorySize(string path)
    {
        var info = new DirectoryInfo(path);
        long size = 0;

        try
        {
            foreach (var file in info.GetFiles())
            {
                size += file.Length;
            }

            foreach (var directory in info.GetDirectories())
            {
                size += GetDirectorySize(directory.FullName);
            }
        }
        catch (Exception e)
        {
            string errorMessage = $"An error occurred while calculating the size of directory {path}: {e.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
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