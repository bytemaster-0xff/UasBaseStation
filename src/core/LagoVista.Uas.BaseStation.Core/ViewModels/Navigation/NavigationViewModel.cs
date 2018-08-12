using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels.Navigation
{
    public class NavigationViewModel : ViewModelBase
    {
        IConnectedUasManager _connectedUasManager;
        IMissionPlanner _missionPlanner;
        INavigation _navigation;

        public NavigationViewModel(IConnectedUasManager connectedUasManager, IMissionPlanner missionPlanner, INavigation navigation)
        {
            _missionPlanner = missionPlanner;
            _connectedUasManager = connectedUasManager;

            RemovewWaypointCommand = new RelayCommand(RemoveWaypoint);
            EditWaypointCommand = new RelayCommand(EditWaypoint);
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);
        }

        public void RemoveWaypoint()
        {

        }

        public void EditWaypoint(Object seq)
        {
            Waypoint = Mission.Waypoints.Where(wp => wp.Sequence == Convert.ToInt32(seq)).FirstOrDefault();
        }


        public void MoveUp()
        {

        }


        public void MoveDown()
        {

        }

        public override async Task InitAsync()
        {
            var response = await _missionPlanner.GetWayPointsAsync(_connectedUasManager.Active);
            if (response.Successful)
            {
                Mission = response.Result;
            }
            await base.InitAsync();
        }

        private Mission _mission;
        public Mission Mission
        {
            get { return _mission; }
            set { Set(ref _mission, value); }
        }

        public RelayCommand RemovewWaypointCommand { get; }
        public RelayCommand EditWaypointCommand { get; }
        public RelayCommand MoveUpCommand { get; }
        public RelayCommand MoveDownCommand { get; }

        private Waypoint _waypoint;
        public Waypoint Waypoint
        {
            get => _waypoint;
            set => Set(ref _waypoint, value);
        }
    }
}
