using LagoVista.Core.Commanding;
using LagoVista.Core.Models.Geo;
using LagoVista.Core.ViewModels;
using LagoVista.Uas.Core.FlightRecorder;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.Services
{
    public class Navigation : ViewModelBase, INavigation
    {
        IConnectedUasManager _connectedUasManager;
        IMissionPlanner missionPlanner;
        IFlightRecorder _flightRecorder;

        public Navigation(IConnectedUasManager connectUasManager, IMissionPlanner missionPlanner, IFlightRecorder flightRecorder)
        {
            _connectedUasManager = connectUasManager;
            _flightRecorder = flightRecorder;
            MissionPlanner = missionPlanner;


            ArmCommand = new RelayCommand(Arm);
            DisarmCommand = new RelayCommand(Disarm);

            TakeoffCommand = new RelayCommand(Takeoff);
            LandCommand = new RelayCommand(Land);
        }

        public override async Task InitAsync()
        {
            var result = await MissionPlanner.GetWayPointsAsync(_connectedUasManager.Active);
            Mission = result.Result;
        }

        public void GoToLocation()
        {

        }

        private void SendMessage(UasMessage msg)
        {
            if (_connectedUasManager.Active != null)
            {
                _connectedUasManager.Active.Transport.SendMessage(msg);
            }
        }

        UasManualControl _msg;

        public void SetVirtualJoystick(short throttle, short pitch, short roll, short yaw)
        {

            var msg = new UasManualControl()
            {
                R = yaw,
                X = pitch,
                Y = roll,
                Z = throttle,

            };

            if (_msg == null || (_msg.X != pitch || _msg.Y != roll || _msg.Z != throttle || _msg.R != yaw))
            {
                SendMessage(msg);
                _msg = msg;
            }
        }

        public void Takeoff()
        {
            _flightRecorder.Publish("Take off");
            var msg = UasCommands.NavTakeoff(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, float.NaN, 0, TakeoffAltitude);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void StartMission()
        {
            _flightRecorder.Publish("Start Mission");
            var msg = UasCommands.MissionStart(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 5);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void Arm()
        {
            _flightRecorder.Publish("Arm");
            var msg = UasCommands.ComponentArmDisarm(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 1);
            SendMessage(msg);
        }

        public void Disarm()
        {
            _flightRecorder.Publish("Disarm");
            var msg = UasCommands.ComponentArmDisarm(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 1);
            SendMessage(msg);
        }

        public void Land()
        {
            _flightRecorder.Publish("Land");
            var msg = UasCommands.NavLandLocal(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0);
            SendMessage(msg);
        }

        public void ReturnToHome()
        {
            _flightRecorder.Publish("Return Home");
            var msg = UasCommands.NavReturnToLaunch(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId);
            SendMessage(msg);
        }

        public void GetHomePosition()
        {
            var msg = UasCommands.GetHomePosition(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0);
            SendMessage(msg);
        }

        private float _talepffAltitude = 2;
        public float TakeoffAltitude
        {
            get { return _talepffAltitude; }
            set { Set(ref _talepffAltitude, value); }
        }

        Models.Mission _mission;
        public Models.Mission Mission
        {
            get { return _mission; }
            set { Set(ref _mission, value); }
        }

        public RelayCommand ArmCommand { get; }
        public RelayCommand DisarmCommand { get; }

        public RelayCommand TakeoffCommand { get; }
        public RelayCommand LandCommand { get; }

        public IMissionPlanner MissionPlanner { get; }
    }
}
