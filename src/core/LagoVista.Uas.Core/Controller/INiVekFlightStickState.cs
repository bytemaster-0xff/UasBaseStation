namespace LagoVista.Uas.Core.Controller
{
    public interface INiVekFlightStickState
    {
        bool TakeOff { get; set; }
        bool Land { get; set; }
        bool IsConnected { get; set; }
        short Rudder { get; set; }
        short Throttle { get; set; }
        short Roll { get; set; }
        short Pitch { get; set; }
        bool BegingManualCameraControl { get; set; }
        bool EndManualCameraControl { get; set; }
        short CameraGimbleX { get; set; }
        short CameraGimbleY { get; set; }
        bool EndMission { get; set; }
        bool PauseMission { get; set; }
        bool ContinueMission { get; set; }
        bool StartMission { get; set; }
        bool ReturnToHome { get; set; }
        bool NextWaypoint { get; set; }
        bool PreviousWaypoint { get; set; }
        bool AltitudeHold { get; set; }


        bool LeftShoulder { get; set; }
        bool LeftThumb { get; set; }
        short LeftTrigger { get; set; }
        bool Back { get; set; }
        INiVekDPad DPad { get; set; }
        bool RightShoulder { get; set; }
        bool RightThumb { get; set; }
        short RightTrigger { get; set; }
    }

    public interface INiVekDPad
    {
        bool Left { get; set; }

        bool Up { get; set; }

        bool Right { get; set; }

        bool Down { get; set; }
    }
}