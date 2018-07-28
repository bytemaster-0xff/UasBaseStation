using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class MainViewModel : AppViewModelBase
    {
        IUas _apmDrone;
        IUasAdapter _droneAdapter;
        IMissionPlanner _planner;

        public MainViewModel(IMissionPlanner planner)
        {
            TelemetryLink = new SerialPortTransport();
            //TelemetryLink.MessageParsed += _telemeteryLink_MessageParsed;
            OpenSerialPortCommand = new RelayCommand(HandleConnectClick, CanPressConnect);
            GetWaypointsCommand = new RelayCommand(GetWaypoints, CanDoConnectedStuff);

            Title = "Kevin";



            _planner = planner;
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
            return TelemetryLink.IsConected;
        }

        public async override Task InitAsync()
        {
            Ports = await DeviceManager.GetSerialPortsAsync();
            await base.InitAsync();
        }

        public void HandleConnectClick()
        {
            if (TelemetryLink.IsConected)
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
            SelectedPort.BaudRate = 57600;
            await (TelemetryLink as SerialPortTransport).OpenAsync(DeviceManager.CreateSerialPort(SelectedPort));
            ConnectMessage = "Disconnect";
            OpenSerialPortCommand.RaiseCanExecuteChanged();
            GetWaypointsCommand.RaiseCanExecuteChanged();
        }

        public async void CloseSerialPort()
        {
            await (TelemetryLink as SerialPortTransport).CloseAsync();
            ConnectMessage = "Connect";
            OpenSerialPortCommand.RaiseCanExecuteChanged();
            GetWaypointsCommand.RaiseCanExecuteChanged();
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

        public ITransport TelemetryLink { get; }
    }
}
