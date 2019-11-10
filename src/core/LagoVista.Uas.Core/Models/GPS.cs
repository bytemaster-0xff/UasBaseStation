using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.MavLink;
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

        private UInt16 _courseOverGround;
        public UInt16 CourseOverGround
        {
            get { return _courseOverGround; }
            set { Set(ref _courseOverGround, value); }
        }


        public string _fixType;
        public string FixType
        {
            get { return _fixType; }
            set { Set(ref _fixType, value); }
        }

        public float _hdop;
        public float HDOP
        {
            get { return _hdop; }
            set { Set(ref _hdop, value); }
        }

        public float _vdop;
        public float VDOP
        {
            get { return _vdop; }
            set { Set(ref _vdop, value); }
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

        public void Update(UasGpsRawInt gps)
        {
            HeadingAccuracy = gps.HdgAcc;
            VelocityAccuracy = gps.VelAcc;
            VerticalAccuracy = gps.VAcc;
            HorizontalAccuracy = gps.HAcc;
            SateliteCount = gps.SatellitesVisible;
            CourseOverGround = gps.Cog;
            HDOP = gps.Eph;
            VDOP = gps.Epv;

            FixType = ((GpsFixType)gps.FixType).ToString();

            Location = new GeoLocation(gps.Lat.ToLatLon(), gps.Lon.ToLatLon());
        }
     
        public void Update(UasGps2Raw gps)
        {
            Location = new GeoLocation(gps.Lat.ToLatLon(), gps.Lon.ToLatLon());

            SateliteCount = gps.SatellitesVisible;
            CourseOverGround = gps.Cog;
            HDOP = gps.Eph;
            VDOP = gps.Epv;
        }

    }
}
