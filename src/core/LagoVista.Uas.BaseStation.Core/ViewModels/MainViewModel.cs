using LagoVista.Client.Core.Resources;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Client.Core.ViewModels.Orgs;
using LagoVista.Client.Core.ViewModels.Other;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models;
using LagoVista.Uas.BaseStation.Core.ViewModels.Uas;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Drones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class MainViewModel : AppViewModelBase
    {
        IUas _apmDrone;
        IUasMessageAdapter _droneAdapter;
        IMissionPlanner _planner;
        IHeartBeatManager _heartBeatManager;
        ITelemetryService _telemetryService;

        public MainViewModel(IMissionPlanner planner, IHeartBeatManager heartBeatManager, ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
            _heartBeatManager = heartBeatManager;

            TelemetryLink = new SerialPortTransport(DispatcherServices);
            //TelemetryLink.MessageParsed += _telemeteryLink_MessageParsed;
            OpenSerialPortCommand = new RelayCommand(HandleConnectClick, CanPressConnect);
            GetWaypointsCommand = new RelayCommand(GetWaypoints, CanDoConnectedStuff);
            StartDataStreamsCommand = new RelayCommand(() => _telemetryService.Start(_apmDrone, TelemetryLink), CanDoConnectedStuff);
            StopDataStreamsCommand = new RelayCommand(() => _telemetryService.Stop(TelemetryLink), CanDoConnectedStuff);

            Title = "Kevin";

            _apmDrone = new APM(null);

            _planner = planner;

            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<UasTypeManagerViewModel>(this)),
                    Name = "Settings",
                    FontIconKey = "fa-users"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<UserOrgsViewModel>(this)),
                    Name = ClientResources.MainMenu_SwitchOrgs,
                    FontIconKey = "fa-users"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<ChangePasswordViewModel>(this)),
                    Name = ClientResources.MainMenu_ChangePassword,
                    FontIconKey = "fa-key"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<InviteUserViewModel>(this)),
                    Name = ClientResources.MainMenu_InviteUser,
                    FontIconKey = "fa-user"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<AboutViewModel>(this)),
                    Name = "About",
                    FontIconKey = "fa-info"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => Logout()),
                    Name = ClientResources.Common_Logout,
                    FontIconKey = "fa-sign-out"
                }
            };
        }

        private void _telemeteryLink_MessageParsed(object sender, UasMessage msg)
        {
            _droneAdapter.UpdateUas(_apmDrone, msg);
        }

        public bool CanPressConnect()
        {
            return _serialPortInfo != null;
        }

        public bool CanDoConnectedStuff()
        {
            return TelemetryLink.IsConnected;
        }

        public async override Task InitAsync()
        {
            Ports = await DeviceManager.GetSerialPortsAsync();
            await base.InitAsync();
        }

        public void HandleConnectClick()
        {
            if (TelemetryLink.IsConnected)
            {
                CloseSerialPort();
            }
            else
            {
                OpenSerialPort();
            }
        }

        public async void GetWaypoints()
        {
            await _planner.GetWayPoints(_apmDrone, TelemetryLink);
        }

        public async void OpenSerialPort()
        {
            SelectedPort.BaudRate = 115200;
            var port = DeviceManager.CreateSerialPort(SelectedPort);
            TelemetryLink.Initialize();
            await (TelemetryLink as SerialPortTransport).OpenAsync(port);
            ConnectMessage = "Disconnect";

            OpenSerialPortCommand.RaiseCanExecuteChanged();
            GetWaypointsCommand.RaiseCanExecuteChanged();
            StartDataStreamsCommand.RaiseCanExecuteChanged();
            StopDataStreamsCommand.RaiseCanExecuteChanged();

            _heartBeatManager.Start(TelemetryLink, TimeSpan.FromSeconds(1));
        }

        public async void CloseSerialPort()
        {
            await (TelemetryLink as SerialPortTransport).CloseAsync();
            ConnectMessage = "Connect";

            OpenSerialPortCommand.RaiseCanExecuteChanged();
            GetWaypointsCommand.RaiseCanExecuteChanged();
            StartDataStreamsCommand.RaiseCanExecuteChanged();
            StopDataStreamsCommand.RaiseCanExecuteChanged();
        }


        private SerialPortInfo _serialPortInfo;
        public SerialPortInfo SelectedPort
        {
            get { return _serialPortInfo; }
            set
            {
                Set(ref _serialPortInfo, value);
                OpenSerialPortCommand.RaiseCanExecuteChanged();
                GetWaypointsCommand.RaiseCanExecuteChanged();
            }
        }

        private String _connectMessage = "Connect";
        public String ConnectMessage
        {
            get { return _connectMessage; }
            set { Set(ref _connectMessage, value); }
        }

        IEnumerable<SerialPortInfo> _ports;
        public IEnumerable<SerialPortInfo> Ports
        {
            get { return _ports; }
            set { Set(ref _ports, value); }
        }


        public String Title
        { get; set; }

        public RelayCommand OpenSerialPortCommand { get; }
        public RelayCommand GetWaypointsCommand { get; }
        public RelayCommand StartDataStreamsCommand { get; }
        public RelayCommand StopDataStreamsCommand { get; }

        public ITransport TelemetryLink { get; }
    }
}
