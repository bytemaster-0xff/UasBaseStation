using LagoVista.Core.Commanding;
using LagoVista.Core.IOC;
using LagoVista.Core.Models;
using LagoVista.Core.Networking.WiFi;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Drones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class DroneConnectViewModel : BaseViewModel
    {
        private readonly IWiFiAdaptersService _adaptersService;
        private readonly IWiFiNetworksService _networkService;
        private readonly IDeviceManager _deviceManager;
        private readonly IConnectedUasManager _connectedUasManager;

        public DroneConnectViewModel(IWiFiAdaptersService adaptersSerivce, IWiFiNetworksService networkService,
            IConnectedUasManager connectedUasManager, IDeviceManager deviceManager)
        {
            this._adaptersService = adaptersSerivce ?? throw new ArgumentNullException(nameof(adaptersSerivce));
            this._networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
            this._deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            this._connectedUasManager = connectedUasManager ?? throw new ArgumentNullException(nameof(connectedUasManager));

            this.ConnectAPMCommand = new RelayCommand(ConnectAPM);
            this.ConnectTelloCommand = new RelayCommand(ConnectTello);
            this.ConnectMavicAirCommand = new RelayCommand(ConnectMavicAir);
            this.ConnectMavicProCommand = new RelayCommand(ConnectMavicPro);
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
        public WiFiAdapter CurrentAdapter
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

        SerialPortInfo _mavicProSerialPort;
        public SerialPortInfo MavicProSerialPort
        {
            get => _mavicProSerialPort;
            set => Set(ref _mavicProSerialPort, value);
        }

        SerialPortInfo _apmSerialPort;
        public SerialPortInfo ApmSerialPort
        {
            get => _apmSerialPort;
            set => Set(ref _apmSerialPort, value);
        }

        WiFiConnection _mavicAirSSID;
        public WiFiConnection MavicAirSSID
        {
            get => _mavicAirSSID;
            set => Set(ref _mavicAirSSID, value);
        }

        WiFiConnection _telloSSID;
        public WiFiConnection TelloSSID
        {
            get => _telloSSID;
            set => Set(ref _telloSSID, value);
        }

        public void ConnectTello()
        {

        }

        public void ConnectMavicPro()
        {

        }

        public async void ConnectAPM()
        {
            var transport = new SerialPortTransport(DispatcherServices);

            var port = _deviceManager.CreateSerialPort(ApmSerialPort);
            await transport.OpenAsync(port);
            var apm = new APM(transport, null);

            _connectedUasManager.SetActive(new ConnectedUas(apm, transport));
            _connectedUasManager.Active.Transport.Initialize();
            var args = new ViewModelLaunchArgs()
            {
                ParentViewModel = this,
                LaunchType = LaunchTypes.View,
                ViewModelType = typeof(FlightViewModel),
            };

            await ViewModelNavigation.NavigateAsync(args);
        }

        public void ConnectMavicAir()
        {

        }

        public RelayCommand ConnectAPMCommand { get; }

        public RelayCommand ConnectMavicAirCommand { get; }

        public RelayCommand ConnectMavicProCommand { get; }

        public RelayCommand ConnectTelloCommand { get; }
    }
}
