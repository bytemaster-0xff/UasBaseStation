using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;

namespace LagoVista.Uas.Core.Models
{
    public class Navigation : GaugeBase
    {
        private float _navRoll;
        private float _navPitch;
        private Int16 _navBearing;
        private Int16 mTargetBearing;
        private UInt16 _waypointDistance;
        private float _altitudeError;
        private float _airSpeedError;
        private float _crossTrackError;
        
        float _targetBearing;
        public float TargetBearing
        {
            get { return _targetBearing; }
            set { Set(ref _targetBearing, value); }
        }

        float _targetAltitude;
        public float TargetAltitude
        {
            get { return _targetAltitude; }
            set { Set(ref _targetAltitude, value); }
        }

        public float NavRoll
        {
            get { return _navRoll; }
            set { Set(ref _navRoll, value); }
        }

        public float NavPitch
        {
            get { return _navPitch; }
            set { Set(ref _navPitch, value); }
        }

        public Int16 NavBearing
        {
            get { return _navBearing; }
            set { Set(ref _navBearing, value); }
        }

        public UInt16 WaypointDistance
        {
            get { return _waypointDistance; }
            set { Set(ref _waypointDistance, value); }
        }

        public float AltitudeError
        {
            get { return _altitudeError; }
            set { Set(ref _altitudeError, value); }
        }
        public float AirspeedError
        {
            get { return _airSpeedError; }
            set { Set(ref _airSpeedError, value); }
        }

        /// <summary>
        /// Current crosstrack error on x-y plane
        /// </summary>
        public float CrossTrackError
        {
            get { return _crossTrackError; }
            set { Set(ref _crossTrackError, value); }
        }

        public void Update(UasNavControllerOutput output)
        {
            AirspeedError = output.AspdError;
            AltitudeError = output.AltError;
            NavBearing = output.NavBearing;
            NavPitch = output.NavPitch;
            NavRoll = output.NavRoll;
            TargetBearing = output.TargetBearing;
            CrossTrackError = output.XtrackError;
            WaypointDistance = output.WpDist;

            GaugeStatus = GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }
    }
}
