using System.Net;
using System.Net.Sockets;
using WindowsCommands.Commands;

namespace WindowsCommands;

public static class PortScanner
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string[] _commonPaths = { "admin", "login", "test", "backup", "index.html" };

    public static async Task ScanAllPortsAsync(string host)
    {
        var tasks = new List<Task>();
        var semaphore = new SemaphoreSlim(100);

        for (int port = 1; port <= 65535; port++)
        {
            await semaphore.WaitAsync();

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        await tcpClient.ConnectAsync(host, port);

                        string service = GetPortService(port);
                        Console.WriteLine($"Port {port} is open. Service: {service}");
                        ConsoleOutputSaver.SaveOutput($"Port {port} is open. Service: {service}");

                        await ExecuteHttpEnumScriptAsync(host, port);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Port {port} is closed");
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    public static async Task ScanPortRangeAsync(string host, int startPort, int endPort)
    {
        var tasks = new List<Task>();
        var semaphore = new SemaphoreSlim(100);

        for (int port = startPort; port <= endPort; port++)
        {
            await semaphore.WaitAsync();

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        await tcpClient.ConnectAsync(host, port);

                        string service = GetPortService(port);
                        Console.WriteLine($"Port {port} is open. Service: {service}");
                        ConsoleOutputSaver.SaveOutput($"Port {port} is open. Service: {service}");

                        await ExecuteHttpEnumScriptAsync(host, port);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Port {port} is closed");
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    public static async Task ScanSinglePortAsync(string host, int port)
    {
        using (TcpClient tcpClient = new TcpClient())
        {
            try
            {
                await tcpClient.ConnectAsync(host, port);

                string service = GetPortService(port);
                Console.WriteLine($"Port {port} is open. Service: {service}");
                ConsoleOutputSaver.SaveOutput($"Port {port} is open. Service: {service}");

                await ExecuteHttpEnumScriptAsync(host, port);
            }
            catch (Exception)
            {
                Console.WriteLine($"Port {port} is closed");
            }
        }
    }

    private static string GetPortService(int port)
    {
        switch (port)
        {
            case 80:
                return "HTTP protocol proxy service";
            case 135:
                return "DCE endpoint resolutionnetbios-ns";
            case 445:
                return "Security service";
            case 1025:
                return "NetSpy.698(YAI)";
            case 8080:
                return "HTTP protocol proxy service";
            case 8081:
                return "HTTP protocol proxy service";
            case 3128:
                return "HTTP protocol proxy service";
            case 9080:
                return "HTTP protocol proxy service";
            case 1080:
                return "SOCKS protocol proxy service";
            case 21:
                return "FTP (file transfer) protocol proxy service";
            case 23:
                return "Telnet (remote login) protocol proxy service";
            case 443:
                return "HTTPS protocol proxy service";
            case 69:
                return "TFTP protocol proxy service";
            case 22:
                return "SSH, SCP, port redirection protocol proxy service";
            case 25:
                return "SMTP protocol proxy service";
            case 110:
                return "POP3 protocol proxy service";
            default:
                return "Unknown service";
        }
    }

    private static async Task ExecuteHttpEnumScriptAsync(string host, int port)
    {
        foreach (var path in _commonPaths)
        {
            await CheckPathAsync($"http://{host}:{port}", path);
        }
    }

    private static async Task CheckPathAsync(string baseUrl, string path)
    {
        var fullPath = $"{baseUrl}/{path}";

        try
        {
            var response = await _httpClient.GetAsync(fullPath, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Found path: {fullPath}");
                ConsoleOutputSaver.SaveOutput($"Found path: {fullPath}");

                if (path.EndsWith("/"))
                {
                    foreach (var commonPath in _commonPaths)
                    {
                        await CheckPathAsync(fullPath, commonPath);
                    }
                }
            }
            else if (response.StatusCode == HttpStatusCode.Redirect ||
                     response.StatusCode == HttpStatusCode.MovedPermanently)
            {
                var redirectUrl = response.Headers.Location.ToString();
                await CheckPathAsync(redirectUrl, "");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when sending request to {fullPath}: {ex.Message}");
            ConsoleOutputSaver.SaveOutput($"Error when sending request to {fullPath}: {ex.Message}");
        }
    }
}