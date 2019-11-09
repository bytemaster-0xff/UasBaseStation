using LagoVista.Core.Validation;
using LagoVista.Uas.BaseStation.Core.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace LagoVista.Uas.BaseStation.App.Network
{
    //todo: need to report the updated wifi SSID and preferredAdapter back to the model through events
    //https://stackoverflow.com/questions/35243570/programmatically-listing-and-connecting-to-wifi-networks-on-windows-iot-core
    public class WiFiAdapter : IWiFiAdapter
    {
        private bool _isConnected = false;
        private string _ssid = string.Empty;
        private readonly string _defaultSsid;
        private Windows.Devices.WiFi.WiFiAdapter _wifiAdapter;

        public event EventHandler<IWiFiConnection> Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SsidUpdatedEventArgs> SsidUpdated;
        public event EventHandler<WiFiNetworkSelectedEventArgs> WiFiNetworkSelected;

        Timer _timer = new Timer();

        public WiFiAdapter(string defaultSsid)
        {
            _defaultSsid = defaultSsid;
        }

        public async Task<bool> CheckAuthroizationAsync()
        {
            var access = await Windows.Devices.WiFi.WiFiAdapter.RequestAccessAsync();
            return access == WiFiAccessStatus.Allowed;
        }

        public async Task<WiFiAdapterId[]> GetAdapterList()
        {
            var adapters = await DeviceInformation.FindAllAsync(Windows.Devices.WiFi.WiFiAdapter.GetDeviceSelector());
            return adapters.Select(a => new WiFiAdapterId(a.Name, a.Id)).ToArray();
        }

        public async Task<InvokeResult> InitAsync(string ssid, WiFiAdapterId preferredAdapter)
        {
            Debug.WriteLine($"{nameof(WiFiAdapter)}.{nameof(InitAsync)} [{ssid}]");

            _ssid = ssid.ToLower();
            if (_wifiAdapter != null)
            {
                _wifiAdapter.Disconnect();
                _wifiAdapter = null;
            }

            if (preferredAdapter != null && !string.IsNullOrEmpty(preferredAdapter.Id))
            {
                _wifiAdapter = await Windows.Devices.WiFi.WiFiAdapter.FromIdAsync(preferredAdapter.Id);
                if (_wifiAdapter == null)
                {
                    throw new ApplicationException($"preferred adapter '{preferredAdapter.Name}' not found");
                }
                Debug.WriteLine($"preferred adapter '{preferredAdapter.Name}' found");
            }
            else
            {
                Debug.WriteLine($"no preferred adapter requested. will pick the first one identified");
                var wifiAdapterResults = (await DeviceInformation.FindAllAsync(Windows.Devices.WiFi.WiFiAdapter.GetDeviceSelector())).FirstOrDefault();
                if (wifiAdapterResults != null)
                {
                    Debug.WriteLine($"found adapter '{wifiAdapterResults.Name}'");
                    _wifiAdapter = await Windows.Devices.WiFi.WiFiAdapter.FromIdAsync(wifiAdapterResults.Id);
                    WiFiNetworkSelected?.Invoke(this, new WiFiNetworkSelectedEventArgs(new WiFiAdapterId(wifiAdapterResults.Name, wifiAdapterResults.Id)));
                }
            }

            if (_wifiAdapter != null)
            {
                Debug.WriteLine("scanning for tello network....");

                var result = await RefreshAdaptersAsync();

                _timer.Elapsed += Timer_Elapsed;
                _timer.Interval = 5000;
                _timer.Start();

                return result;
            }
            return InvokeResult.FromError("No wireless connections");
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            await RefreshAdaptersAsync();
            _timer.Start();
        }


        public async Task<InvokeResult> RefreshAdaptersAsync()
        {
            var result = InvokeResult.Success;

            //Debug.WriteLine($"{nameof(WiFiAdapter)}.{nameof(RefreshAdaptersAsync)}, connected: {_isConnected}, wifi networks found: {_wifiAdapter.NetworkReport.AvailableNetworks.Count}");

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                await _wifiAdapter.ScanAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"_wifiAdapter.ScanAsync() wifi choked: {ex.Message}");
                //throw;
            }

            if (!_isConnected)
            {
                var network = _ssid != _defaultSsid ?
                    _wifiAdapter.NetworkReport.AvailableNetworks.Where(nw => nw.Ssid.ToLower().CompareTo(_ssid) == 0).FirstOrDefault() :
                    _wifiAdapter.NetworkReport.AvailableNetworks.Where(nw => nw.Ssid.ToLower().StartsWith("tello")).FirstOrDefault();

                if (network != null)
                {
                    if (_ssid == _defaultSsid)
                    {
                        _ssid = network.Ssid;
                        SsidUpdated?.Invoke(this, new SsidUpdatedEventArgs(_ssid));
                    }
                    Debug.WriteLine($"found tello wifi - ssid: {_ssid}");

                    await _wifiAdapter.ConnectAsync(network, WiFiReconnectionKind.Automatic);
                    _isConnected = true;
                    Connected?.Invoke(this, new WiFiConnection(network));
                }
                else
                {
                    result = InvokeResult.FromError("tello not found");
                }
            }
            else
            {
                var ssidFound = _wifiAdapter.NetworkReport.AvailableNetworks.Count(nw => nw.Ssid.ToLower().CompareTo(_ssid) == 0) > 0;
                //Debug.WriteLine($"ssid '{_ssid}' found: {ssidFound}");
                if (!ssidFound)
                {
                    DumpConnection();
                }
            }

            sw.Stop();

            //Debug.WriteLine($"Time to scan networks {sw.ElapsedMilliseconds}");

            return result;
        }

        private void DumpConnection()
        {
            _wifiAdapter.Disconnect();
            Debug.WriteLine($"connection with {_ssid} lost");
            _isConnected = false;
            Disconnected?.Invoke(this, null);
        }

        public Task<InvokeResult> ConnectAsync(IWiFiConnection connection)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IWiFiConnection>> GetAvailableConnections()
        {
            throw new NotImplementedException();
        }
    }
}