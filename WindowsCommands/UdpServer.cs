using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WindowsCommands;

public static class UdpServer
{
    private static UdpClient _server;
    private static bool _isRunning;

    public static void StartUdpServer(int port)
    {
        _server = new UdpClient(port);
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
        _isRunning = true;

        while (_isRunning)
        {
            Console.WriteLine("Waiting for client...");
            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = _server.Receive(ref client);
            string message = Encoding.UTF8.GetString(data);
            Console.WriteLine($"Received data: {message} from {client}");
        }
    }

    public static void Stop()
    {
        _isRunning = false;
        _server.Close();
    }
}