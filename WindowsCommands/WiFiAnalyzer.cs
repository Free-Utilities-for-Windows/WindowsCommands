using System.Text;
using NativeWifi;
using WindowsCommands.Commands;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class WiFiAnalyzer
{
    public static void ScanNetworks(string userInput)
    {
        WlanClient client = new WlanClient();

        while (true)
        {
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                wlanIface.Scan();
                Thread.Sleep(1000);

                Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                foreach (Wlan.WlanBssEntry network in wlanBssEntries)
                {
                    string securityType = GetSecurityType(wlanIface, network.dot11Ssid);

                    if (userInput == "all" || (userInput == "open" && securityType == "IEEE80211_Open"))
                    {
                        string networkDetails = string.Format(
                            "Found network with SSID {0}, BSSID (MAC): {1}, Signal strength: {2} dBm, BSS Type: {3}, PHY Type: {4}, Channel: {5}, Security: {6}.",
                            GetStringForSSID(network.dot11Ssid),
                            GetMacAddress(network.dot11Bssid),
                            network.rssi,
                            network.dot11BssType,
                            network.dot11BssPhyType,
                            GetChannelFromFrequency(network.chCenterFrequency),
                            securityType
                        );

                        Console.WriteLine(networkDetails);
                        StaticFileLogger.LogInformation(networkDetails);
                    }
                }
            }

            Thread.Sleep(5000);
        }
    }

    public static void ConnectToNetwork(string targetSsid)
    {
        WlanClient client = new WlanClient();

        foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
        {
            wlanIface.Scan();
            Thread.Sleep(1000);

            Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
            foreach (Wlan.WlanBssEntry network in wlanBssEntries)
            {
                string securityType = GetSecurityType(wlanIface, network.dot11Ssid);

                if (GetStringForSSID(network.dot11Ssid) == targetSsid && securityType == "IEEE80211_Open")
                {
                    string profileName = GetStringForSSID(network.dot11Ssid);
                    Wlan.WlanConnectionMode connectionMode = Wlan.WlanConnectionMode.Profile;
                    Wlan.Dot11BssType bssType = Wlan.Dot11BssType.Infrastructure;
                    wlanIface.Connect(connectionMode, bssType, profileName);
                    string connectingMessage = "Connecting to network: " + targetSsid;
                    Console.WriteLine(connectingMessage);
                    StaticFileLogger.LogInformation(connectingMessage);

                    if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected &&
                        GetStringForSSID(wlanIface.CurrentConnection.wlanAssociationAttributes.dot11Ssid) ==
                        targetSsid)
                    {
                        string successMessage = "Successfully connected to network: " + targetSsid;
                        Console.WriteLine(successMessage);
                        StaticFileLogger.LogInformation(successMessage);
                    }
                    else
                    {
                        string failureMessage = "Failed to connect to network: " + targetSsid;
                        Console.WriteLine(failureMessage);
                        StaticFileLogger.LogError(failureMessage);
                    }
                }
            }
        }
    }

    public static string GetStringForSSID(Wlan.Dot11Ssid ssid)
    {
        return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
    }

    public static string GetMacAddress(byte[] macAddr)
    {
        var str = new string[(int)macAddr.Length];
        for (int i = 0; i < macAddr.Length; i++)
        {
            str[i] = macAddr[i].ToString("x2");
        }

        return string.Join(":", str);
    }

    public static string GetSecurityType(WlanClient.WlanInterface wlanIface, Wlan.Dot11Ssid ssid)
    {
        foreach (Wlan.WlanAvailableNetwork network in wlanIface.GetAvailableNetworkList(0))
        {
            if (GetStringForSSID(network.dot11Ssid) == GetStringForSSID(ssid))
            {
                return network.dot11DefaultAuthAlgorithm.ToString();
            }
        }

        return "IEEE80211_Open";
    }

    public static int GetChannelFromFrequency(uint frequency)
    {
        uint[] channelFrequencies24GHz =
        {
            2412000, 2417000, 2422000, 2427000, 2432000, 2437000, 2442000, 2447000, 2452000, 2457000, 2462000, 2467000,
            2472000, 2484000
        };

        for (int channel = 1; channel <= 14; channel++)
        {
            if (frequency == channelFrequencies24GHz[channel - 1])
            {
                return channel;
            }
        }

        return (int)((frequency - 5000000) / 5000);
    }
}