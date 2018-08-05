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
using LagoVista.Uas.Core.Services;
using LagoVista.Uas.Drones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class MainViewModel : AppViewModelBase
    {
        IMissionPlanner _planner;
        IHeartBeatManager _heartBeatManager;
        ITelemetryService _telemetryService;

        public MainViewModel(IMissionPlanner planner, IHeartBeatManager heartBeatManager, ITelemetryService telemetryService, IConnectedUasManager connectedUasManager)
        {
            _telemetryService = telemetryService;
            _heartBeatManager = heartBeatManager;

            Connections = connectedUasManager;

            var transport = new SerialPortTransport(DispatcherServices);

            Connections.Active = new ConnectedUas(new APM(null), transport);
            Connections.All.Add(Connections.Active);

            Connections.Active.Transport.OnMessageReceived += _telemeteryLink_MessageParsed;

            //TelemetryLink.MessageParsed += _telemeteryLink_MessageParsed;
            OpenSerialPortCommand = new RelayCommand(HandleConnectClick, CanPressConnect);
            ShowMissionPlannerCommand = new RelayCommand(() => ViewModelNavigation.NavigateAsync<Missions.MissionPlannerViewModel>(this), CanDoConnectedStuff);
            StartDataStreamsCommand = new RelayCommand(() => _telemetryService.Start(Connections.Active.Uas, Connections.Active.Transport), CanDoConnectedStuff);
            StopDataStreamsCommand = new RelayCommand(() => _telemetryService.Stop(Connections.Active.Transport), CanDoConnectedStuff);
            BeginCalibrationCommand = new RelayCommand(() => ViewModelNavigation.NavigateAsync<Calibration.AccCalibrationViewModel>(this), CanDoConnectedStuff);
            FlyNowCommand = new RelayCommand(() => ViewModelNavigation.NavigateAsync<HudViewModel>(this), CanDoConnectedStuff);

            Title = "UAS NuvIoT Connector";

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
            Connections.Active.Uas.Update(msg);
        }

        public bool CanPressConnect()
        {
            return _serialPortInfo != null;
        }

        public bool CanDoConnectedStuff()
        {
            return Connections.Active.Transport.IsConnected;
        }

        public async override Task InitAsync()
        {
            Ports = await DeviceManager.GetSerialPortsAsync();
            await base.InitAsync();
        }

        public void HandleConnectClick()
        {
            if (Connections.Active.Transport.IsConnected)
            {
                CloseSerialPort();
            }
            else
            {
                OpenSerialPort();
            }
        }
       
        public async void OpenSerialPort()
        {
            SelectedPort.BaudRate = 115200;
            var port = DeviceManager.CreateSerialPort(SelectedPort);
            Connections.Active.Transport.Initialize();
            await (Connections.Active.Transport as SerialPortTransport).OpenAsync(port);
            ConnectMessage = "Disconnect";

            OpenSerialPortCommand.RaiseCanExecuteChanged();
            ShowMissionPlannerCommand.RaiseCanExecuteChanged();
            StartDataStreamsCommand.RaiseCanExecuteChanged();
            StopDataStreamsCommand.RaiseCanExecuteChanged();
            BeginCalibrationCommand.RaiseCanExecuteChanged();
            FlyNowCommand.RaiseCanExecuteChanged();

            _heartBeatManager.Start(Connections.Active.Transport, TimeSpan.FromSeconds(1));
        }

        public async void CloseSerialPort()
        {
            _heartBeatManager.Stop();
            await (Connections.Active.Transport as SerialPortTransport).CloseAsync();
            ConnectMessage = "Connect";

            OpenSerialPortCommand.RaiseCanExecuteChanged();
            ShowMissionPlannerCommand.RaiseCanExecuteChanged();
            StartDataStreamsCommand.RaiseCanExecuteChanged();
            StopDataStreamsCommand.RaiseCanExecuteChanged();
            BeginCalibrationCommand.RaiseCanExecuteChanged();
            FlyNowCommand.RaiseCanExecuteChanged();
        }


        private SerialPortInfo _serialPortInfo;
        public SerialPortInfo SelectedPort
        {
            get { return _serialPortInfo; }
            set
            {
                Set(ref _serialPortInfo, value);
                OpenSerialPortCommand.RaiseCanExecuteChanged();
                ShowMissionPlannerCommand.RaiseCanExecuteChanged();
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
        public RelayCommand ShowMissionPlannerCommand { get; }
        public RelayCommand StartDataStreamsCommand { get; }
        public RelayCommand StopDataStreamsCommand { get; }
        public RelayCommand BeginCalibrationCommand { get; }
        public RelayCommand FlyNowCommand { get; }

        public IConnectedUasManager Connections { get; }


    }
}
