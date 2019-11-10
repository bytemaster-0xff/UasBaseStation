using LagoVista.Uas.Core.Controller;
using System;

namespace LagoVista.Uas.Core
{
    public interface INiVekFlightStick
    {
        INiVekFlightStickState State { get; }

        event EventHandler<INiVekFlightStickState> StateUpdated;
        event EventHandler BeginManualCameraControl;
        event EventHandler ContinueMission;
        event EventHandler EndManualCameraControl;
        event EventHandler EndMission;
        event EventHandler Land;
        event EventHandler NextWaypoint;
        event EventHandler PauseMission;
        event EventHandler PreviousWaypoint;
        event EventHandler ReturnToHome;
        event EventHandler StartMission;
        event EventHandler TakeOff;
    }
}