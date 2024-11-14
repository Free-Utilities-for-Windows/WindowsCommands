using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class DeviceScanner
{
    public static List<Device> ScanDevices()
    {
        var devices = new List<Device>();

        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if (IsValidNetworkInterface(networkInterface))
                {
                    var device = CreateDeviceFromNetworkInterface(networkInterface);
                    devices.Add(device);
                    StaticFileLogger.LogInformation($"Device found: {device.Name}, {device.MacAddress}");
                }
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"Error scanning devices: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }

        return devices;
    }

    private static bool IsValidNetworkInterface(NetworkInterface networkInterface)
    {
        return networkInterface.OperationalStatus == OperationalStatus.Up &&
               networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
               networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
               networkInterface.Description != "Teredo Tunneling Pseudo-Interface";
    }

    private static Device CreateDeviceFromNetworkInterface(NetworkInterface networkInterface)
    {
        var device = new Device
        {
            Name = networkInterface.Name,
            Type = networkInterface.NetworkInterfaceType.ToString(),
            Description = networkInterface.Description,
            Speed = networkInterface.Speed,
            OperationalStatus = networkInterface.OperationalStatus.ToString(),
            SupportsMulticast = networkInterface.SupportsMulticast,
            MacAddress = GetMacAddress(networkInterface)
        };

        var ipProperties = networkInterface.GetIPProperties();
        foreach (var ipInfo in ipProperties.UnicastAddresses)
        {
            if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
            {
                device.GlobalDeviceIp = new IpConfiguration
                {
                    Address = ipInfo.Address.ToString(),
                    SubnetMask = ipInfo.IPv4Mask.ToString()
                };
            }
        }

        return device;
    }

    private static string GetMacAddress(NetworkInterface networkInterface)
    {
        var macBytes = networkInterface.GetPhysicalAddress()?.GetAddressBytes();
        return macBytes != null && macBytes.Length == 6
            ? string.Join(":", macBytes.Select(b => b.ToString("X2")))
            : "00:00:00:00:00:00";
    }

    public static void PrintDeviceInformation(List<Device> scannedDevices)
    {
        foreach (var device in scannedDevices)
        {
            string deviceInfo = GetDeviceInformation(device);
            string ipInfo = GetIpInformation(device.GlobalDeviceIp);
            Console.WriteLine(deviceInfo);
            Console.WriteLine(ipInfo);
            Console.WriteLine($"Speed: {device.Speed}");
            Console.WriteLine($"Operational Status: {device.OperationalStatus}");
            Console.WriteLine($"Supports Multicast: {device.SupportsMulticast}");
            Console.WriteLine("-------------------------------------------");

            StaticFileLogger.LogInformation(deviceInfo);
            StaticFileLogger.LogInformation(ipInfo);
        }
    }

    private static string GetDeviceInformation(Device device)
    {
        return $"Name: {device.Name}\nType: {device.Type}\nDescription: {device.Description}";
    }

    private static string GetIpInformation(IpConfiguration ipConfig)
    {
        return $"IP Address: {ipConfig.Address}\nSubnet Mask: {ipConfig.SubnetMask}";
    }

    public class IpConfiguration
    {
        private readonly List<string> _dnsHosts;

        public IpConfiguration()
        {
            _dnsHosts = new List<string>();
            VlanId = 1;
        }

        private string _address;

        public string Address
        {
            get => _address;
            set => _address = IsValidIpAddress(value) ? value : throw new ArgumentException("Invalid IP Address.");
        }

        private string _subnetMask;

        public string SubnetMask
        {
            get => _subnetMask;
            set => _subnetMask = IsValidIpAddress(value) ? value : throw new ArgumentException("Invalid Subnet Mask.");
        }

        private string _defaultGateway;

        public string DefaultGateway
        {
            get => _defaultGateway;
            set => _defaultGateway = IsValidIpAddress(value)
                ? value
                : throw new ArgumentException("Invalid Default Gateway.");
        }

        public IReadOnlyList<string> DnsHosts => _dnsHosts;

        private int _vlanId;

        public int VlanId
        {
            get => _vlanId;
            set => _vlanId = (value >= 1 && value <= 4094)
                ? value
                : throw new ArgumentException("Invalid VLAN ID. Valid range is 1-4094.");
        }

        public void AddDnsHost(string dnsHost)
        {
            _dnsHosts.Add(dnsHost);
        }

        public void RemoveDnsHost(string dnsHost)
        {
            _dnsHosts.Remove(dnsHost);
        }

        private bool IsValidIpAddress(string ipAddress)
        {
            return System.Net.IPAddress.TryParse(ipAddress, out _);
        }
    }

    public class Device
    {
        public Device()
        {
            Id = Guid.NewGuid();
            GlobalDeviceIp = new IpConfiguration();
            NetworkCredential = new NetworkCredential();
        }

        public Guid Id { get; set; }

        private string _type;

        public string Type
        {
            get => _type;
            set => _type = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Type cannot be null or empty.");
        }

        private string _manufacturer;

        public string Manufacturer
        {
            get => _manufacturer;
            set => _manufacturer = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Manufacturer cannot be null or empty.");
        }

        private string _model;

        public string Model
        {
            get => _model;
            set => _model = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Model cannot be null or empty.");
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => _name = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Name cannot be null or empty.");
        }

        private string _location;

        public string Location
        {
            get => _location;
            set => _location = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Location cannot be null or empty.");
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => _description = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Description cannot be null or empty.");
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set => _notes = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("Notes cannot be null or empty.");
        }

        private string _macAddress;

        public string MacAddress
        {
            get => _macAddress;
            set => _macAddress = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("MacAddress cannot be null or empty.");
        }

        private long _speed;

        public long Speed
        {
            get => _speed;
            set => _speed = value > 0 ? value : throw new ArgumentException("Speed must be greater than zero.");
        }

        private string _operationalStatus;

        public string OperationalStatus
        {
            get => _operationalStatus;
            set => _operationalStatus = !string.IsNullOrEmpty(value)
                ? value
                : throw new ArgumentException("OperationalStatus cannot be null or empty.");
        }

        private bool _supportsMulticast;

        public bool SupportsMulticast
        {
            get => _supportsMulticast;
            set => _supportsMulticast = value;
        }

        public NetworkCredential NetworkCredential { get; set; }
        public IpConfiguration GlobalDeviceIp { get; set; }
    }
}