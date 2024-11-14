using WindowsCommands.Logger;

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
                string fileInfoMessage = $"Deleting file: {fileInfo.FullName}";
                Console.WriteLine(fileInfoMessage);
                StaticFileLogger.LogInformation(fileInfoMessage);

                try
                {
                    File.Delete(fileInfo.FullName);
                    string successMessage = $"Successfully deleted file: {fileInfo.FullName}";
                    StaticFileLogger.LogInformation(successMessage);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"An error occurred while deleting file {fileInfo.FullName}: {ex.Message}";
                    Console.WriteLine(errorMessage);
                    StaticFileLogger.LogError(errorMessage);
                }
            }
        }
    }
}