using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class GPS : ModelBase
    {
        private GeoLocation _location;
        public GeoLocation Location
        {
            get { return _location; } 
            set { Set(ref _location, value); }
        }

        private int _sateliteCount;
        public int SateliteCount
        {
            get { return _sateliteCount; }
            set { Set(ref _sateliteCount, value); }
        }

        public float _hdop;
        public float HDOP
        {
            get { return _hdop; }
            set { Set(ref _hdop, value); }
        }

        public float _horizontalAccuracy;
        public float HorizontalAccuracy
        {
            get { return _horizontalAccuracy; }
            set { Set(ref _horizontalAccuracy, value); }
        }

        public float _verticalAccuracy;
        public float VerticalAccuracy
        {
            get { return _verticalAccuracy; }
            set { Set(ref _verticalAccuracy, value); }
        }

        public float _velocityAccuracy;
        public float VelocityAccuracy
        {
            get { return _velocityAccuracy; }
            set { Set(ref _velocityAccuracy, value); }
        }

        public float _headingAccuracy;
        public float HeadingAccuracy
        {
            get { return _headingAccuracy; }
            set { Set(ref _headingAccuracy, value); }
        }

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set { Set(ref _time, value); }
        }

    }
}
