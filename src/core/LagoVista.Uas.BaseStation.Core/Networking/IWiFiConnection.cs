using System;

namespace LagoVista.Uas.BaseStation.Core.Networking
{
    public interface IWiFiConnection
    {
        String SSID { get; }
        byte Signal {get;}
    }
}
