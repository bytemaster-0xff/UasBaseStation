using System;

namespace LagoVista.Uas.BaseStation.Core.Networking
{
    public class SsidUpdatedEventArgs : EventArgs
    {
        public SsidUpdatedEventArgs(string ssid)
        {
            if (string.IsNullOrEmpty(ssid)) throw new ArgumentNullException(nameof(ssid));
            SSID = ssid;
        }

        public string SSID { get; }
    }
}
