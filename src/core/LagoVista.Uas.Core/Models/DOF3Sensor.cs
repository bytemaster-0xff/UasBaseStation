using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public enum DOFSensorType
    {
        Gyro,
        Accelerometer,
        Magnometer,
    }

    public class DOF3Sensor : ModelBase
    {
        private Int16 _x;
        public Int16 X
        {
            get { return _x; }
            set { Set(ref _x, value); }
        }

        private Int16 _y;
        public Int16 Y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }

        private Int16 _z;
        public Int16 Z
        {
            get { return _z; }
            set { Set(ref _z, value); }
        }

        private float _offsetx;
        public float OffsetX
        {
            get { return _offsetx; }
            set { Set(ref _offsetx, value); }
        }

        private float _offsetY;
        public float OffsetY
        {
            get { return _offsetY; }
            set { Set(ref _offsetY, value); }
        }

        private float _offsetZ;
        public float OffsetZ
        {
            get { return _offsetZ; }
            set { Set(ref _offsetZ, value); }
        }


        private float _magneticDeclination;
        public float MagneticDeclination
        {
            get { return _magneticDeclination; }
            set { Set(ref _magneticDeclination, value); }
        }

        public void Update(UasSensorOffsets offsets, DOFSensorType sensorType)
        {
            switch(sensorType)
            {
                case DOFSensorType.Accelerometer:
                    OffsetX = offsets.AccelCalX;
                    OffsetY = offsets.AccelCalY;
                    OffsetZ = offsets.AccelCalZ;
                    break;
                case DOFSensorType.Gyro:
                    OffsetX = offsets.GyroCalX;
                    OffsetY = offsets.GyroCalY;
                    OffsetZ = offsets.GyroCalZ;
                    break;
                case DOFSensorType.Magnometer:
                    MagneticDeclination = offsets.MagDeclination;
                    OffsetX = offsets.MagOfsX;
                    OffsetY = offsets.MagOfsY;
                    OffsetZ = offsets.MagOfsZ;
                    break;
            }
        }
    }
}
