using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Mission : ViewModelBase
    {


        public Mission()
        {
            Waypoints = new ObservableCollection<Waypoint>();

            RemovewWaypointCommand = new RelayCommand(RemoveWaypoint);
            EditWaypointCommand = new RelayCommand(EditWaypoint);
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);
            Status = "Ready";
        }

        public ObservableCollection<Waypoint> Waypoints { get; }

        public void RemoveWaypoint(Object seq)
        {
            var waypointSequence = Convert.ToInt32(seq);
            Waypoints.RemoveAt(waypointSequence);
            ushort idx = 0;
            foreach(var wp in Waypoints)
            {
                wp.Sequence = idx++;
            }
        }

        public void EditWaypoint(Object seq)
        {
            Waypoint = Waypoints.Where(wp => wp.Sequence == Convert.ToInt32(seq)).FirstOrDefault();
        }

        private Waypoint _waypoint;
        public Waypoint Waypoint
        {
            get => _waypoint;
            set => Set(ref _waypoint, value);
        }


        public void MoveUp(Object seq)
        {

        }


        public void MoveDown(Object seq)
        {

        }

        private String _status;
        public string Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        Waypoint _selectedWaypoint;
        public Waypoint CurrentWaypoint
        {
            get { return _selectedWaypoint; }
            set { Set(ref _selectedWaypoint, value); }
        }


        public RelayCommand RemovewWaypointCommand { get; }
        public RelayCommand EditWaypointCommand { get; }
        public RelayCommand MoveUpCommand { get; }
        public RelayCommand MoveDownCommand { get; }

    }
}
