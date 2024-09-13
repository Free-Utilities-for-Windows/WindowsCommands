using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;
using WindowsCommands.Commands;

namespace WindowsCommands;

public static class YTDownloader
{
    public static void DownloadVideo(string videoUrl, string mode, int? desiredQualityIndex = null)
    {
        if (!videoUrl.StartsWith("https://www.youtube.com/watch?v="))
        {
            throw new Exception("Wrong URL format");
        }

        var youTube = YouTube.Default;
        var videos = youTube.GetAllVideos(videoUrl);
        var videoList = videos.Where(v => v.AdaptiveKind == AdaptiveKind.Video || v.AdaptiveKind == AdaptiveKind.None).ToList();

        var selectedVideo = desiredQualityIndex.HasValue ? videoList.ElementAtOrDefault(desiredQualityIndex.Value) : videoList.LastOrDefault();

        string baseFilePath = Path.Combine(ConsoleOutputSaver.GetFolderPath(), selectedVideo.FullName);
        string filePath = CreateUniqueFilePath(baseFilePath);

        Console.WriteLine("Downloading...");
        byte[] videoBytes = selectedVideo.GetBytes();
        File.WriteAllBytes(filePath, videoBytes);
        Console.WriteLine("Download completed.");
        ConsoleOutputSaver.SaveOutput($"Downloaded video: {filePath}");

        if (mode == "--mp3")
        {
            VideoConverter.ConvertToMp3(filePath);
        }
    }

    public static async Task DownloadVideos(List<string> videoUrls, string mode)
    {
        Console.WriteLine("Available qualities will be shown for the first video. Please select desired quality:");
        int? desiredQualityIndex = null;

        if (videoUrls.Any())
        {
            string firstVideoUrl = videoUrls.First();
            var youTube = YouTube.Default;
            var videos = youTube.GetAllVideos(firstVideoUrl);
            var videoList = videos.Where(v => v.AdaptiveKind == AdaptiveKind.Video || v.AdaptiveKind == AdaptiveKind.None).ToList();

            for (int i = 0; i < videoList.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {videoList[i].Resolution}p {videoList[i].Format} {(videoList[i].AdaptiveKind == AdaptiveKind.None ? "(combined)" : "(video only)")}");
            }

            Console.WriteLine("Enter the number for the desired quality (press Enter for highest quality):");
            string input = Console.ReadLine();
            desiredQualityIndex = string.IsNullOrEmpty(input) ? videoList.Count - 1 : int.Parse(input) - 1;
        }

        Parallel.ForEach(videoUrls, (videoUrl) =>
        {
            try
            {
                DownloadVideo(videoUrl, mode, desiredQualityIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download video {videoUrl}: {ex.Message}");
                ConsoleOutputSaver.SaveOutput($"Failed to download video {videoUrl}: {ex.Message}");
            }
        });
    }

    public static class VideoConverter
    {
        public static void ConvertToMp3(string filePath)
        {
            string mp3FilePath = Path.ChangeExtension(filePath, ".mp3");
            var inputFile = new MediaFile { Filename = filePath };
            var outputFile = new MediaFile { Filename = mp3FilePath };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                engine.Convert(inputFile, outputFile);
            }

            File.Delete(filePath);
            Console.WriteLine("Conversion to MP3 completed.");
            ConsoleOutputSaver.SaveOutput($"Converted to MP3: {mp3FilePath}");
        }
    }

    private static string CreateUniqueFilePath(string baseFilePath)
    {
        string filePath = baseFilePath;
        int count = 1;

        while (File.Exists(filePath))
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(baseFilePath);
            string extension = Path.GetExtension(baseFilePath);
            filePath = Path.Combine(Path.GetDirectoryName(baseFilePath), $"{fileNameWithoutExtension} ({count++}){extension}");
        }

        return filePath;
    }
}