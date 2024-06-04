namespace WindowsCommands;

public static class TempFileCleaner
{
    public static void CleanOldTempFiles()
    {
        var tempPath = Path.GetTempPath();
        var tempFiles = Directory.GetFiles(tempPath, "*.tmp");

        var dateThreshold = DateTime.Now.AddDays(-30);

        foreach (var tempFile in tempFiles)
        {
            var fileInfo = new FileInfo(tempFile);

            if (fileInfo.LastWriteTime <= dateThreshold)
            {
                Console.WriteLine(fileInfo.FullName);
                try
                {
                    File.Delete(fileInfo.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}