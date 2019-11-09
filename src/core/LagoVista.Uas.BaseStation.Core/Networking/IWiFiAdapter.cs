using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.Networking
{
    public interface IWiFiAdapter
    {
        Task<bool> CheckAuthroizationAsync();
        event EventHandler<IWiFiConnection> Connected;
        event EventHandler Disconnected;
        event EventHandler<SsidUpdatedEventArgs> SsidUpdated;
        event EventHandler<WiFiNetworkSelectedEventArgs> WiFiNetworkSelected;

        Task<IEnumerable<IWiFiConnection>> GetAvailableConnections();
        Task<InvokeResult> ConnectAsync(IWiFiConnection connection);
        Task<InvokeResult> InitAsync(string ssid, WiFiAdapterId preferredAdapter);
        Task<InvokeResult> RefreshAdaptersAsync();
        Task<WiFiAdapterId[]> GetAdapterList();
    }
}
