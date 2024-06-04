namespace WindowsCommands.Commands;

public static class ConsoleOutputSaver
{
    private static string folderPath;

    public static void Initialize()
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var projectFolder = Path.Combine(desktop, "WindowsCommands");

        if (!Directory.Exists(projectFolder))
        {
            Directory.CreateDirectory(projectFolder);
        }

        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var timestampFolder = Path.Combine(projectFolder, timestamp);

        Directory.CreateDirectory(timestampFolder);

        folderPath = timestampFolder;
    }

    public static void SaveOutput(string output)
    {
        var filePath = Path.Combine(folderPath, "ConsoleOutput.txt");
        File.AppendAllText(filePath, output + Environment.NewLine);
    }
}