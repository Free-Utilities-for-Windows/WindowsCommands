using System.Management;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class VideoCardInformation
{
    public static void GetVideoCardInfo()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            var videoCardInfos = new List<VideoCardInfo>();

            foreach (ManagementObject queryObj in searcher.Get())
            {
                var videoCardInfo = new VideoCardInfo
                {
                    Model = queryObj["Name"].ToString(),
                    Display = queryObj["CurrentHorizontalResolution"] + "x" + queryObj["CurrentVerticalResolution"],
                    VideoRAM = (Convert.ToUInt64(queryObj["AdapterRAM"]) / (1024 * 1024)) + " MB"
                };
                videoCardInfos.Add(videoCardInfo);
            }

            foreach (var videoCardInfo in videoCardInfos)
            {
                string videoCardInfoMessage = $"Model: {videoCardInfo.Model}\n" +
                                              $"Display: {videoCardInfo.Display}\n" +
                                              $"VideoRAM: {videoCardInfo.VideoRAM}";
                Console.WriteLine(videoCardInfoMessage);
                StaticFileLogger.LogInformation(videoCardInfoMessage);
            }
        }
        catch (ManagementException e)
        {
            string errorMessage = "An error occurred while querying for WMI data: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public class VideoCardInfo
    {
        public string Model { get; set; }
        public string Display { get; set; }
        public string VideoRAM { get; set; }
    }
}