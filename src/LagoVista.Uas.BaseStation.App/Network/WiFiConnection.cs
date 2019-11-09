using LagoVista.Uas.BaseStation.Core.Networking;
using Windows.Devices.WiFi;

namespace LagoVista.Uas.BaseStation.App.Network
{
    public class WiFiConnection : IWiFiConnection
    {
        WiFiAvailableNetwork _network;
        public WiFiConnection(WiFiAvailableNetwork network)
        {
            _network = network;
        }

        public string SSID => _network.Ssid;

        public byte Signal => _network.SignalBars;
    }
}
