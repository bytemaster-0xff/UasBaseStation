using LagoVista.Core.Commanding;
using LagoVista.Core.Models.Geo;
using LagoVista.Core.ViewModels;
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
       
        public Navigation(IConnectedUasManager connectUasManager, IMissionPlanner missionPlanner)
        {
            _connectedUasManager = connectUasManager;
            MissionPlanner = missionPlanner;


            ArmCommand = new RelayCommand(Arm);
            DisarmCommand = new RelayCommand(Disarm);

            TakeoffCommand = new RelayCommand(Takeoff);
            LandCommand = new RelayCommand(Land);
        }

        public void Takeoff()
        {
            var msg =  UasCommands.NavTakeoff(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, float.NaN, 0, TakeoffAltitude);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void StartMission()
        {
            var msg = UasCommands.MissionStart(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0,5);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public override async Task InitAsync()
        {
            var result = await MissionPlanner.GetWayPointsAsync(_connectedUasManager.Active);
            Mission = result.Result;
        }

        public void GoToLocation()
        {
            
        }

        public void Arm()
        {
            var msg = UasCommands.ComponentArmDisarm(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 1);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void Disarm()
        {
            var msg = UasCommands.ComponentArmDisarm(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 1);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void Land()
        {
            var msg = UasCommands.NavLandLocal(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void ReturnToHome()
        {
            var msg = UasCommands.NavReturnToLaunch(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void GetHomePosition()
        {
            var msg = UasCommands.GetHomePosition(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0);
            _connectedUasManager.Active.Transport.SendMessage(msg);
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
