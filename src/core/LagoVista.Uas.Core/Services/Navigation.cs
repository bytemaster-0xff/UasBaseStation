using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Services
{
    public class Navigation
    {
        IConnectedUasManager _connectedUasManager;
        public Navigation(IConnectedUasManager connectUasManager)
        {
            _connectedUasManager = connectUasManager;
        }

        public void Takeoff(float altitude)
        {
            UasCommands.NavTakeoff(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, float.NaN, 0, altitude);
        }

        public void GoToLocation(GeoLocation location)
        {
            //      UasCommands.NavWaypoint()
        }

        public void Land()
        {
            UasCommands.NavLandLocal(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0);
        }

        public void ReturnToHome()
        {
            UasCommands.NavReturnToLaunch(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId);
        }

        public void GetHomePosition()
        {
            UasCommands.GetHomePosition(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0);

        }
    }
}
