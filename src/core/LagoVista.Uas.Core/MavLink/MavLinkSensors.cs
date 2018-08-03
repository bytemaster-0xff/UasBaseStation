using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.MavLink
{
 public class MavLinkSensors
    {
        BitArray bitArray = new BitArray(32);

        public bool seen = false;
       
        public MavLinkSensors(uint p)
        {
            seen = true;
            bitArray = new BitArray(new int[] { (int)p });
        }

        public bool Gyro
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dGyro)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dGyro)] = value; }
        }

        public bool Accelerometer
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dAccel)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dAccel)] = value; }
        }

        public bool Compass
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dMag)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dMag)] = value; }
        }

        public bool Barometer
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.AbsolutePressure)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.AbsolutePressure)] = value; }
        }

        public bool DifferentialPressure
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.DifferentialPressure)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.DifferentialPressure)] = value; }
        }

        public bool GPS
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.Gps)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.Gps)] = value; }
        }

        public bool OpticalFlow
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.OpticalFlow)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.OpticalFlow)] = value; }
        }

        public bool MissionPosition
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.VisionPosition)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.VisionPosition)] = value; }
        }

        public bool LaserPosition
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.LaserPosition)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.LaserPosition)] = value; }
        }

        public bool GroundTruth
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.ExternalGroundTruth)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.ExternalGroundTruth)] = value; }
        }

        public bool RateControl
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.AngularRateControl)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.AngularRateControl)] = value; }
        }

        public bool AttitudeStabilization
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.AttitudeStabilization)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.AttitudeStabilization)] = value; }
        }

        public bool YawPosition
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.YawPosition)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.YawPosition)] = value; }
        }

        public bool AltitudeControl
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.ZAltitudeControl)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.ZAltitudeControl)] = value; }
        }

        public bool XYPositionControl
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.XyPositionControl)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.XyPositionControl)] = value; }
        }

        public bool MotorControl
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MotorOutputs)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MotorOutputs)] = value; }
        }

        public bool RCReceiver
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.RcReceiver)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.RcReceiver)] = value; }
        }

        public bool Gyro2
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dGyro2)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dGyro2)] = value; }
        }

        public bool Accel2
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dAccel2)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dAccel2)] = value; }
        }

        public bool Mag2
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dMag2)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor._3dMag2)] = value; }
        }

        public bool GeoFence
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusGeofence)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusGeofence)] = value; }
        }

        public bool AHRS
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusAhrs)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusAhrs)] = value; }
        }

        public bool Terrain
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusTerrain)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusTerrain)] = value; }
        }

        public bool RevThrottle
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusReverseMotor)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusReverseMotor)] = value; }
        }

        public bool Logging
        {
            get { return bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusLogging)]; }
            set { bitArray[ConvertValuetoBitmaskOffset((int)MavSysStatusSensor.MavSysStatusLogging)] = value; }
        }

        int ConvertValuetoBitmaskOffset(int input)
        {
            int offset = 0;
            for (int a = 0; a < sizeof(int) * 8; a++)
            {
                offset = 1 << a;
                if (input == offset)
                    return a;
            }
            return 0;
        }

        public uint Value
        {
            get
            {
                int[] array = new int[1];
                bitArray.CopyTo(array, 0);
                return (uint)array[0];
            }
            set
            {
                seen = true;
                bitArray = new BitArray(new int[] { (int)value });
            }
        }

        public override string ToString()
        {
            return Convert.ToString(Value, 2);
        }
    }
}
