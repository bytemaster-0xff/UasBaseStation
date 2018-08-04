using LagoVista.Core.Models;

namespace LagoVista.Uas.Core.Models
{
    public class Navigation : ModelBase
    {
        public float _roll;
        public float Roll
        {
            get { return _targetBearing; }
            set { Set(ref _targetBearing, value); }
        }

        public float _pitch;
        public float Pitch
        {
            get { return _targetBearing; }
            set { Set(ref _targetBearing, value); }
        }

        public float _bearing;
        public float Bearing
        {
            get { return _targetBearing; }
            set { Set(ref _targetBearing, value); }
        }

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
    }
}
