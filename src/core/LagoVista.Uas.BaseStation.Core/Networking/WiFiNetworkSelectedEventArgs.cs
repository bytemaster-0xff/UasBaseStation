using System;

namespace LagoVista.Uas.BaseStation.Core.Networking
{
    public class WiFiNetworkSelectedEventArgs : EventArgs
    {
        public WiFiNetworkSelectedEventArgs(WiFiAdapterId wiFiAdapaterId) : base()
        {
            WiFiAdapaterId = wiFiAdapaterId ?? throw new ArgumentNullException(nameof(wiFiAdapaterId));
        }

        public WiFiAdapterId WiFiAdapaterId { get; }
    }
}
