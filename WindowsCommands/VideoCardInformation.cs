using System.Management;

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
                var videoCardInfo = new VideoCardInfo();
                videoCardInfo.Model = queryObj["Name"].ToString();
                videoCardInfo.Display = queryObj["CurrentHorizontalResolution"] + "x" + queryObj["CurrentVerticalResolution"];
                videoCardInfo.VideoRAM = (Convert.ToUInt64(queryObj["AdapterRAM"]) / (1024 * 1024)) + " MB";
                videoCardInfos.Add(videoCardInfo);
            }

            foreach (var videoCardInfo in videoCardInfos)
            {
                Console.WriteLine("Model: " + videoCardInfo.Model);
                Console.WriteLine("Display: " + videoCardInfo.Display);
                Console.WriteLine("VideoRAM: " + videoCardInfo.VideoRAM);
            }
        }
        catch (ManagementException e)
        {
            Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
    
    public class VideoCardInfo
    {
        public string Model { get; set; }
        public string Display { get; set; }
        public string VideoRAM { get; set; }
    }
}