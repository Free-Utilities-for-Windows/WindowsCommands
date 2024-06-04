using HtmlAgilityPack;

namespace WindowsCommands;

public static class ImageDownloader
{
    public static async Task DownloadImages(string url)
    {
        var _httpClient = new HttpClient();

        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var images = doc.DocumentNode.Descendants("img")
            .Select(e => e.GetAttributeValue("src", null))
            .Where(src => !string.IsNullOrEmpty(src))
            .Select(src => new Uri(new Uri(url), src).AbsoluteUri)
            .ToList();

        var folder = url.Replace("http://", "").Replace("https://", "").Replace("/", "-").TrimEnd('-');
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), folder);

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);

        foreach (var img in images)
        {
            var fileName = Path.GetFileName(new Uri(img).AbsolutePath);
            var filePath = Path.Combine(path, fileName);

            var imageBytes = await _httpClient.GetByteArrayAsync(img);
            await File.WriteAllBytesAsync(filePath, imageBytes);
        }
    }
}