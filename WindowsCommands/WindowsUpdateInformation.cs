using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class WindowsUpdateInformation
{
    public static void GetWindowsUpdateInfo()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_QuickFixEngineering");
            var updates = searcher.Get();

            var winUpdates = new List<WindowsUpdateInfo>();

            foreach (ManagementObject update in updates)
            {
                DateTime? installDate;

                try
                {
                    installDate = ManagementDateTimeConverter.ToDateTime(update["InstalledOn"].ToString());
                }
                catch
                {
                    installDate = null;
                }

                var winUpdate = new WindowsUpdateInfo
                {
                    HotFixID = update["HotFixID"].ToString(),
                    InstallDate = installDate,
                    Description = update["Description"].ToString(),
                    InstalledBy = update["InstalledBy"].ToString()
                };

                winUpdates.Add(winUpdate);
            }

            winUpdates = winUpdates.OrderByDescending(u => u.InstallDate).ToList();

            foreach (var update in winUpdates)
            {
                string updateInfo = $"HotFixID: {update.HotFixID}, InstallDate: {update.InstallDate:dd.MM.yyyy}, Description: {update.Description}, InstalledBy: {update.InstalledBy}";
                Console.WriteLine(updateInfo);
                StaticFileLogger.LogInformation(updateInfo);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public class WindowsUpdateInfo
    {
        public string HotFixID { get; set; }
        public DateTime? InstallDate { get; set; }
        public string Description { get; set; }
        public string InstalledBy { get; set; }
    }
}