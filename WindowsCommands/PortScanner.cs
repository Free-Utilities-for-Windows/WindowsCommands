using System.Net;
using System.Net.Sockets;
using WindowsCommands.Logger;

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
                        string openPortMessage = $"Port {port} is open. Service: {service}";
                        Console.WriteLine(openPortMessage);
                        StaticFileLogger.LogInformation(openPortMessage);

                        await ExecuteHttpEnumScriptAsync(host, port);
                    }
                }
                catch (Exception ex)
                {
                    string closedPortMessage = $"Port {port} is closed";
                    Console.WriteLine(closedPortMessage);
                    StaticFileLogger.LogError(closedPortMessage);
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
                        string openPortMessage = $"Port {port} is open. Service: {service}";
                        Console.WriteLine(openPortMessage);
                        StaticFileLogger.LogInformation(openPortMessage);

                        await ExecuteHttpEnumScriptAsync(host, port);
                    }
                }
                catch (Exception ex)
                {
                    string closedPortMessage = $"Port {port} is closed";
                    Console.WriteLine(closedPortMessage);
                    StaticFileLogger.LogError(closedPortMessage);
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
                string openPortMessage = $"Port {port} is open. Service: {service}";
                Console.WriteLine(openPortMessage);
                StaticFileLogger.LogInformation(openPortMessage);

                await ExecuteHttpEnumScriptAsync(host, port);
            }
            catch (Exception ex)
            {
                string closedPortMessage = $"Port {port} is closed";
                Console.WriteLine(closedPortMessage);
                StaticFileLogger.LogError(closedPortMessage);
            }
        }
    }

    private static string GetPortService(int port)
    {
        return port switch
        {
            80 => "HTTP protocol proxy service",
            135 => "DCE endpoint resolutionnetbios-ns",
            445 => "Security service",
            1025 => "NetSpy.698(YAI)",
            8080 => "HTTP protocol proxy service",
            8081 => "HTTP protocol proxy service",
            3128 => "HTTP protocol proxy service",
            9080 => "HTTP protocol proxy service",
            1080 => "SOCKS protocol proxy service",
            21 => "FTP (file transfer) protocol proxy service",
            23 => "Telnet (remote login) protocol proxy service",
            443 => "HTTPS protocol proxy service",
            69 => "TFTP protocol proxy service",
            22 => "SSH, SCP, port redirection protocol proxy service",
            25 => "SMTP protocol proxy service",
            110 => "POP3 protocol proxy service",
            _ => "Unknown service",
        };
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
                string foundPathMessage = $"Found path: {fullPath}";
                Console.WriteLine(foundPathMessage);
                StaticFileLogger.LogInformation(foundPathMessage);

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
            string errorMessage = $"Error when sending request to {fullPath}: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}