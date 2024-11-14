using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class CleanerUp
{
    public static void RunCleanupTasks()
    {
        try
        {
            EmptyRecycleBin();
            CleanDownloadsFolder();
            CleanCookies();
            CleanRemnantDriverFiles();
            ResetDnsResolverCache();
            CleanOldFiles();
            CleanTraceFiles();
            CleanHistory();
            CleanTempFolder();
            CleanPrefetchFolder();
            CleanWindowsTempFolder();
            CleanLogFiles();
            CleanEventLogs();
            EraseInternetExplorerHistory();
            RunDISMOperations();

            string successMessage = "All cleanup tasks completed successfully.";
            Console.WriteLine(successMessage);
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error running cleanup tasks: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    [DllImport("Shell32.dll")]
    private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);

    private const uint SHERB_NOCONFIRMATION = 0x00000001;
    private const uint SHERB_NOPROGRESSUI = 0x00000002;
    private const uint SHERB_NOSOUND = 0x00000004;

    private static void EmptyRecycleBin()
    {
        try
        {
            uint flags = SHERB_NOCONFIRMATION | SHERB_NOPROGRESSUI | SHERB_NOSOUND;
            SHEmptyRecycleBin(IntPtr.Zero, null, flags);
            StaticFileLogger.LogInformation("Recycle bin emptied successfully.");
        }
        catch (Exception ex)
        {
            string errorMessage = "Error emptying recycle bin: " + ex.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void CleanDownloadsFolder()
    {
        string userProfile = Environment.GetEnvironmentVariable("USERPROFILE")
                             ?? throw new InvalidOperationException("USERPROFILE environment variable is not set.");

        string downloadsPath = Path.Combine(userProfile, "Downloads");
        CleanDirectory(downloadsPath, "Downloads");
    }

    private static void CleanOldFiles()
    {
        string userProfile = Environment.GetEnvironmentVariable("USERPROFILE")
                             ?? throw new InvalidOperationException("USERPROFILE environment variable is not set.");

        string[] oldFilePatterns = { "*.old", "*.bak", "*.tmp" };
        string[] directoriesToClean =
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            userProfile
        };

        foreach (var dir in directoriesToClean)
        {
            foreach (var pattern in oldFilePatterns)
            {
                DeleteFilesByPattern(dir, pattern);
            }
        }
    }

    private static void CleanTraceFiles()
    {
        string userProfile = Environment.GetEnvironmentVariable("USERPROFILE")
                             ?? throw new InvalidOperationException("USERPROFILE environment variable is not set.");

        string[] traceFilePatterns = { "*.trace" };
        string[] directoriesToClean =
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            userProfile
        };

        foreach (var dir in directoriesToClean)
        {
            foreach (var pattern in traceFilePatterns)
            {
                DeleteFilesByPattern(dir, pattern);
            }
        }
    }

    private static void CleanCookies()
    {
        string userProfile = Environment.GetEnvironmentVariable("USERPROFILE")
                             ?? throw new InvalidOperationException("USERPROFILE environment variable is not set.");

        string cookiesPath = Path.Combine(userProfile, "AppData", "Roaming", "Microsoft", "Windows", "Cookies");
        if (Directory.Exists(cookiesPath))
        {
            CleanDirectory(cookiesPath, "Cookies");
        }
    }

    private static void CleanHistory()
    {
        string userProfile = Environment.GetEnvironmentVariable("USERPROFILE")
                             ?? throw new InvalidOperationException("USERPROFILE environment variable is not set.");

        string historyPath = Path.Combine(userProfile, "AppData", "Local", "Microsoft", "Windows", "History");
        if (Directory.Exists(historyPath))
        {
            CleanDirectory(historyPath, "History");
        }
    }

    private static void CleanRemnantDriverFiles()
    {
        string userProfile = Environment.GetEnvironmentVariable("USERPROFILE")
                             ?? throw new InvalidOperationException("USERPROFILE environment variable is not set.");

        string[] driverPaths =
        {
            Path.Combine(userProfile, "AMD"),
            Path.Combine(userProfile, "NVIDIA"),
            Path.Combine(userProfile, "INTEL")
        };

        foreach (var driverPath in driverPaths)
        {
            if (Directory.Exists(driverPath))
            {
                CleanDirectory(driverPath, "Remnant driver files");
            }
        }
    }

    private static void CleanTempFolder()
    {
        CleanDirectory(Path.GetTempPath(), "Temp folder");
    }

    private static void CleanPrefetchFolder()
    {
        string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        string prefetchPath = Path.Combine(windowsPath, "Prefetch");
        CleanDirectory(prefetchPath, "Prefetch");
    }

    private static void CleanWindowsTempFolder()
    {
        string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        string windowsTempPath = Path.Combine(windowsPath, "Temp");
        CleanDirectory(windowsTempPath, "Temp");
    }

    private static void CleanLogFiles()
    {
        string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        string[] logPaths =
        {
            Path.Combine(windowsPath, "Logs"),
            Path.Combine(windowsPath, "Panther"),
            Path.Combine(windowsPath, "SoftwareDistribution", "Download"),
            Path.Combine(programDataPath, "Microsoft", "Windows", "WER", "Temp")
        };

        foreach (var logPath in logPaths)
        {
            if (Directory.Exists(logPath))
            {
                CleanDirectory(logPath, "Log files");
            }
        }
    }

    private static void CleanEventLogs()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "wevtutil.exe",
                Arguments = "cl Application",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();

            Process.Start(new ProcessStartInfo
            {
                FileName = "wevtutil.exe",
                Arguments = "cl System",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();

            string successMessage = "Event logs cleaned successfully.";
            Console.WriteLine(successMessage);
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error cleaning event logs: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void ResetDnsResolverCache()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "ipconfig.exe",
                Arguments = "/flushdns",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();

            string successMessage = "DNS resolver cache reset successfully.";
            Console.WriteLine(successMessage);
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error resetting DNS resolver cache: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void EraseInternetExplorerHistory()
    {
        try
        {
            Console.WriteLine("Attempting to erase Internet Explorer temporary data...");

            Process process = new Process();
            process.StartInfo.FileName = "rundll32.exe";
            process.StartInfo.Arguments = "inetcpl.cpl,ClearMyTracksByProcess 4351";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();

            string successMessage = "Internet Explorer temporary data erased successfully.";
            Console.WriteLine(successMessage);
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to erase Internet Explorer temporary data: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void RunDISMOperations()
    {
        try
        {
            Console.WriteLine("Running DISM to clean old service pack files...");

            Process process = new Process();
            process.StartInfo.FileName = "dism.exe";
            process.StartInfo.Arguments = "/online /cleanup-Image /spsuperseded";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                string successMessage = "DISM operation completed successfully.";
                Console.WriteLine(successMessage);
                StaticFileLogger.LogInformation(successMessage);
            }
            else
            {
                string errorMessage = $"DISM operation failed. Exit code: {process.ExitCode}";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to run DISM operation: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void CleanDirectory(string path, string folderName)
    {
        try
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    TryDeleteFile(file);
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    TryDeleteDirectory(dir);
                }

                string successMessage = $"{folderName} cleaned successfully.";
                Console.WriteLine(successMessage);
                StaticFileLogger.LogInformation(successMessage);
            }
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (IOException ex) when (IsFileLocked(ex))
        {
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error cleaning {folderName}: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void TryDeleteFile(FileInfo file)
    {
        const int MaxRetries = 1;
        const int DelayBetweenRetries = 500;

        for (int i = 0; i < MaxRetries; i++)
        {
            try
            {
                file.Delete();
                string successMessage = $"File {file.FullName} deleted successfully.";
                Console.WriteLine(successMessage);
                StaticFileLogger.LogInformation(successMessage);
                return;
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException ex) when (IsFileLocked(ex))
            {
                Thread.Sleep(DelayBetweenRetries);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error deleting file {file.FullName}: {ex.Message}";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
                return;
            }
        }

        string finalErrorMessage = $"Could not delete file {file.FullName} after {MaxRetries} attempts due to it being in use by another process.";
        Console.WriteLine(finalErrorMessage);
        StaticFileLogger.LogError(finalErrorMessage);
    }

    private static void TryDeleteDirectory(DirectoryInfo dir)
    {
        try
        {
            dir.Delete(true);
            string successMessage = $"Directory {dir.FullName} deleted successfully.";
            Console.WriteLine(successMessage);
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (IOException ex) when (IsFileLocked(ex))
        {
        }
        catch (IOException)
        {
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error deleting directory {dir.FullName}: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static void DeleteFilesByPattern(string directory, string pattern)
    {
        try
        {
            if (Directory.Exists(directory))
            {
                var enumerationOptions = new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                };

                var files = Directory.GetFiles(directory, pattern, enumerationOptions);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                        string successMessage = $"File {file} deleted successfully.";
                        Console.WriteLine(successMessage);
                        StaticFileLogger.LogInformation(successMessage);
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    catch (IOException ex) when (IsFileLocked(ex))
                    {
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Error deleting file '{file}': {ex.Message}";
                        Console.WriteLine(errorMessage);
                        StaticFileLogger.LogError(errorMessage);
                    }
                }

                string finalSuccessMessage = $"Files with pattern {pattern} deleted successfully in {directory}.";
                Console.WriteLine(finalSuccessMessage);
                StaticFileLogger.LogInformation(finalSuccessMessage);
            }
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error deleting files with pattern {pattern} in {directory}: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    private static bool IsFileLocked(IOException exception)
    {
        int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
        return errorCode == 32 || errorCode == 33;
    }
}