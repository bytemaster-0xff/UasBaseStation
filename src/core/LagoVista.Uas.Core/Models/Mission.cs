using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Mission
    {
        public Mission()
        {
            Waypoints = new ObservableCollection<Waypoint>();
        }

        public ObservableCollection<Waypoint> Waypoints { get; }
    }
}
