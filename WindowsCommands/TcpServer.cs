using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WindowsCommands;

public static class TcpServer
{
    private static TcpListener _server;
    private static bool _isRunning;

    public static void StartTcpServer(int port)
    {
        _server = new TcpListener(IPAddress.Any, port);
        Start();
        Console.WriteLine($"Server started on port {port}. Press any key to stop...");
        Console.ReadKey();
        Stop();
    }

    public static void Start()
    {
        Thread thread = new Thread(new ThreadStart(Run));
        thread.Start();
    }

    private static void Run()
    {
        _server.Start();
        _isRunning = true;

        while (_isRunning)
        {
            Console.WriteLine("Waiting for client...");
            var client = _server.AcceptTcpClient();
            Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];

            while (client.Connected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    break;
                }

                string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (!string.IsNullOrWhiteSpace(data))
                {
                    Console.WriteLine("Received data: " + data);
                }
            }
        }
    }

    public static void Stop()
    {
        _isRunning = false;
        _server.Stop();
    }
}