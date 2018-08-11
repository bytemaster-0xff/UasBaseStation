using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Resources;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LagoVista.Uas.Core.Models
{
    public class SensorList
    {
        public class OnboardSensor
        {
            public OnboardSensor(String name, MavSysStatusSensor sensor)
            {
                Sensor = sensor;
                Name = name;
            }

            public MavSysStatusSensor Sensor { get; private set; }
            public String Name { get; private set; }
            public int Mask { get { return (int)Sensor; } }

        }

        private List<OnboardSensor> _onboardSensors = new List<OnboardSensor>()
        {
            new OnboardSensor(CoreResources.Sensor_Accelerometer, MavSysStatusSensor._3dAccel),
            new OnboardSensor(CoreResources.Sensor_Accelerometer2, MavSysStatusSensor._3dAccel2),
            new OnboardSensor(CoreResources.Sensor_AltitudeControl, MavSysStatusSensor.ZAltitudeControl),
            new OnboardSensor(CoreResources.Sensor_AttitudeStabilization, MavSysStatusSensor.AttitudeStabilization),
            new OnboardSensor(CoreResources.Sensor_AHRS, MavSysStatusSensor.MavSysStatusAhrs),
            new OnboardSensor(CoreResources.Sensor_Barometer, MavSysStatusSensor.AbsolutePressure),
            new OnboardSensor(CoreResources.Sensor_Compass, MavSysStatusSensor._3dMag),
            new OnboardSensor(CoreResources.Sensor_Compass2, MavSysStatusSensor._3dMag2),
            new OnboardSensor(CoreResources.Sensor_DifferentialPressure, MavSysStatusSensor.DifferentialPressure),
            new OnboardSensor(CoreResources.Sensor_GeoFence, MavSysStatusSensor.MavSysStatusGeofence),
            new OnboardSensor(CoreResources.Sensor_GPS, MavSysStatusSensor.Gps),
            new OnboardSensor(CoreResources.Sensor_GroundTruth, MavSysStatusSensor.ExternalGroundTruth),
            new OnboardSensor(CoreResources.Sensor_Gyro, MavSysStatusSensor._3dGyro),
            new OnboardSensor(CoreResources.Sensor_Terrain, MavSysStatusSensor.MavSysStatusTerrain),
            new OnboardSensor(CoreResources.Sensor_Logging, MavSysStatusSensor.MavSysStatusLogging),
            new OnboardSensor(CoreResources.Sensor_Gryo2, MavSysStatusSensor._3dGyro2),
            new OnboardSensor(CoreResources.Sensor_LaserPosition, MavSysStatusSensor.LaserPosition),
            new OnboardSensor(CoreResources.Sensor_MissionPosition, MavSysStatusSensor.VisionPosition),
            new OnboardSensor(CoreResources.Sensor_OpticalFlow, MavSysStatusSensor.OpticalFlow),
            new OnboardSensor(CoreResources.Sensor_RateControl, MavSysStatusSensor.AngularRateControl),
            new OnboardSensor(CoreResources.Sensor_RCReceiver, MavSysStatusSensor.AngularRateControl),
            new OnboardSensor(CoreResources.Sensor_RevThrottle, MavSysStatusSensor.MavSysStatusReverseMotor),
            new OnboardSensor(CoreResources.Sensor_MotorControl, MavSysStatusSensor.MotorOutputs),
            new OnboardSensor(CoreResources.Sensor_XYPositionControl, MavSysStatusSensor.XyPositionControl),
            new OnboardSensor(CoreResources.Sensor_YawPosition, MavSysStatusSensor.YawPosition),
        };

        public SensorList()
        {
            Sensors = new ObservableCollection<Sensor>();
        }

        public ObservableCollection<Sensor> Sensors { get; private set; }

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

        public void Update(UasSysStatus sysStatus)
        {
            var healthArray = new BitArray(new int[] { (int)sysStatus.OnboardControlSensorsHealth });
            var enabledArray = new BitArray(new int[] { (int)sysStatus.OnboardControlSensorsEnabled });
            var presentArray = new BitArray(new int[] { (int)sysStatus.OnboardControlSensorsPresent });

            foreach (var onboardSensor in _onboardSensors)
            {
                var sensor = Sensors.Where(sns => sns.SysStatusSensor == onboardSensor.Sensor).FirstOrDefault();
                if (sensor == null)
                {
                    sensor = new Sensor(onboardSensor.Sensor, onboardSensor.Name);
                    Sensors.Add(sensor);
                }

                sensor.Present = presentArray[ConvertValuetoBitmaskOffset(onboardSensor.Mask)];
                sensor.Enabled = enabledArray[ConvertValuetoBitmaskOffset(onboardSensor.Mask)];
                sensor.Healthy = healthArray[ConvertValuetoBitmaskOffset(onboardSensor.Mask)];
                sensor.TimeStamp = DateTime.Now;
            }

            Sensors = new ObservableCollection<Sensor>(Sensors.OrderByDescending(snsr => snsr.Present));
        }
    }
}
