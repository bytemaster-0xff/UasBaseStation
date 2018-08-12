using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;

namespace LagoVista.Uas.Core.Models
{
    public enum CoordSystems
    {
        GeoCoord,
        Relative,
        Terray
    }

    public class Waypoint : ModelBase
    {
        private UInt16 _sequenceq;
        private MavCmd _command;
        private byte _current;
        private byte _autoContinue;
        private float _param1;
        private float _param2;
        private float _param3;
        private float _param4;
        private float _x;
        private float _y;
        private float _z;
        private byte _missionType;

        internal static Waypoint Create(UasMissionItemInt rec)
        {
            var wp = new Waypoint()
            {
                Sequence = rec.Seq,
                Frame = (MavFrame)rec.Frame,
                X = rec.Y.ToLatLon(), // From Mav Link Lon comes on Y
                Y = rec.X.ToLatLon(), // From Mav Link Lat comes in X
                Z = rec.Z,
                Param1 = rec.Param1,
                Param2 = rec.Param2,
                Param3 = rec.Param3,
                Param4 = rec.Param4,
                Command = (MavCmd)rec.Command,
                Current = rec.Current
            };

            return wp;
        }

        public UInt16 Sequence
        {
            get { return _sequenceq; }
            set { _sequenceq = value; RaisePropertyChanged(); }
        }

        private MavFrame _frame;
        public MavFrame Frame
        {
            get => _frame;
            set => Set(ref _frame, value);
        }

        /// <summary>
        /// The scheduled action for the waypoint.
        /// </summary>
        public MavCmd Command
        {
            get { return _command; }
            set { _command = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// false:0, true:1
        /// </summary>
        public byte Current
        {
            get { return _current; }
            set { _current = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Autocontinue to next waypoint
        /// </summary>
        public byte Autocontinue
        {
            get { return _autoContinue; }
            set { _autoContinue = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM1, see MAV_CMD enum
        /// </summary>
        public float Param1
        {
            get { return _param1; }
            set { _param1 = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM2, see MAV_CMD enum
        /// </summary>
        public float Param2
        {
            get { return _param2; }
            set { _param2 = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM3, see MAV_CMD enum
        /// </summary>
        public float Param3
        {
            get { return _param3; }
            set { _param3 = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM4, see MAV_CMD enum
        /// </summary>
        public float Param4
        {
            get { return _param4; }
            set { _param4 = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM6 / y position: local: x position in meters * 1e4, global: longitude in degrees *10^7
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame.
        /// </summary>
        public float Z
        {
            get { return _z; }
            set { _z = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Mission type.
        /// </summary>
        public byte MissionType
        {
            get { return _missionType; }
            set { _missionType = value; RaisePropertyChanged(); }
        }
    }
}
