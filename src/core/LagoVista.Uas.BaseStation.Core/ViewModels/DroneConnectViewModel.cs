using LagoVista.Core.IOC;
using LagoVista.Core.Models;
using LagoVista.Core.Networking.WiFi;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class DroneConnectViewModel : BaseViewModel
    {
        private readonly IWiFiAdapters _adaptersService;
        private readonly IWiFiNetworks _networkService;
        private readonly IDeviceManager _deviceManager;

        public DroneConnectViewModel(IWiFiAdapters adaptersSerivce, IWiFiNetworks networkService, IDeviceManager deviceManager)
        {
            this._adaptersService = adaptersSerivce ?? throw new ArgumentNullException(nameof(adaptersSerivce));
            this._networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
            this._deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
        }

        public override async Task InitAsync()
        {
            this.Adapters = await _adaptersService.GetAdapterListAsync();
            this.SerialPorts = await _deviceManager.GetSerialPortsAsync();

            this.CurrentAdapter = this.Adapters.FirstOrDefault();
            await base.InitAsync();
        }

        private async void ScanNetworks(WiFiAdapter adapter)
        {
            await this._networkService.StartAsync(this.CurrentAdapter);
        }

        private ObservableCollection<WiFiAdapter> _adapters;
        public ObservableCollection<WiFiAdapter> Adapters
        {
            get => _adapters;
            set => Set(ref _adapters, value);
        }

        public ObservableCollection<WiFiConnection> Networks => _networkService.AllConnections;


        ObservableCollection<SerialPortInfo> _serialPorts;
        public ObservableCollection<SerialPortInfo> SerialPorts
        {
            get => _serialPorts;
            set => Set(ref _serialPorts, value);
        }

        WiFiAdapter _currentAdapter;
        WiFiAdapter CurrentAdapter
        {
            get => _currentAdapter;
            set
            {
                Set(ref _currentAdapter, value);
                if (value != null)
                {
                    ScanNetworks(value);
                }
            }
        }

        WiFiConnection _mavicAirConnection;
        public WiFiConnection MavicAirConnection
        {
            get => _mavicAirConnection;
            set => Set(ref _mavicAirConnection, value);
        }

        WiFiConnection _telloConnection;
        public WiFiConnection TelloConnection
        {
            get => _telloConnection;
            set => Set(ref _telloConnection, value);
        }

        public void ConnectToTello()
        {

        }
    }
}
