using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;
using WindowsCommands.Commands;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class YTDownloader
{
        public static void DownloadVideo(string videoUrl, string mode, int? desiredQualityIndex = null)
    {
        try
        {
            if (!videoUrl.StartsWith("https://www.youtube.com/watch?v="))
            {
                throw new Exception("Wrong URL format");
            }

            var youTube = YouTube.Default;
            var videos = youTube.GetAllVideos(videoUrl);
            var videoList = videos.Where(v => v.AdaptiveKind == AdaptiveKind.Video || v.AdaptiveKind == AdaptiveKind.None).ToList();

            if (!videoList.Any())
            {
                throw new Exception("No available videos found for the provided URL.");
            }

            var selectedVideo = desiredQualityIndex.HasValue ? videoList.ElementAtOrDefault(desiredQualityIndex.Value) : videoList.LastOrDefault();

            string baseFilePath = Path.Combine(StaticFileLogger.GetLogFolderPath(), selectedVideo.FullName);
            string filePath = CreateUniqueFilePath(baseFilePath);

            Console.WriteLine("Downloading...");
            StaticFileLogger.LogInformation($"Downloading video from URL: {videoUrl}");
            byte[] videoBytes = selectedVideo.GetBytes();
            File.WriteAllBytes(filePath, videoBytes);
            Console.WriteLine("Download completed.");
            StaticFileLogger.LogInformation($"Download completed: {filePath}");

            if (mode == "--mp3")
            {
                VideoConverter.ConvertToMp3(filePath);
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to download video {videoUrl}: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
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

            if (!videoList.Any())
            {
                Console.WriteLine("No available videos found for the first URL.");
                return;
            }

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
                string errorMessage = $"Failed to download video {videoUrl}: {ex.Message}";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
            }
        });
    }

    public static class VideoConverter
    {
        public static void ConvertToMp3(string filePath)
        {
            try
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
                string successMessage = $"Conversion to MP3 completed: {mp3FilePath}";
                Console.WriteLine(successMessage);
                StaticFileLogger.LogInformation(successMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Failed to convert to MP3: {ex.Message}";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
            }
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