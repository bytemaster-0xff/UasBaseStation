using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;

namespace LagoVista.Uas.Drones
{
    public class APMAdapter : IUasMessageAdapter
    {
        public void UpdateUas(IUas uas, UasMessage message)
        {
            var apm = uas as APM;

            switch (message.MessageId)
            {
                case Core.MavLink.UasMessages.Heartbeat:
                    break;
                case Core.MavLink.UasMessages.GpsRawInt:
                    var gpsMsg = message as UasGpsRawInt;
                    uas.Location = new LagoVista.Core.Models.Geo.GeoLocation()
                    {
                        Altitude = gpsMsg.Alt,
                        Latitude = gpsMsg.Lat / 10000000.0f,
                        Longitude = gpsMsg.Lon / 10000000.0f
                    };

                    break;

                case UasMessages.SysStatus:
                    var status = message as UasSysStatus;
                    uas.Sensors.Update(status);

                    break;

                case UasMessages.Attitude:
                    var att = message as UasAttitude;
                    uas.Pitch = att.Pitch.ToDegrees();
                    uas.PitchSpeed = att.Pitchspeed;
                    uas.Roll = att.Roll.ToDegrees();
                    uas.RollSpeed = att.Rollspeed;
                    uas.Yaw = att.Yaw.ToDegrees();
                    uas.YawSpeed = att.Yawspeed;
                    break;

                case UasMessages.MagCalProgress:
                    /* Compass Calibration Progress  */
                    break;
                case UasMessages.CompassmotStatus:
                    /* Compass Calibration Status  */
                    break;
                case UasMessages.EkfStatusReport:
                    var ekfStatus = message as UasEkfStatusReport;

                    break;
                case UasMessages.VfrHud:
                    /* For fixed wing */
                    break;
                case Core.MavLink.UasMessages.RawImu:
                    var imu = message as UasRawImu;
                    uas.Acc.X = imu.Xacc;
                    uas.Acc.Y = imu.Yacc;
                    uas.Acc.Z = imu.Zacc;

                    uas.Gyro.X = imu.Xgyro;
                    uas.Gyro.Y = imu.Ygyro;
                    uas.Gyro.Z = imu.Zgyro;

                    uas.Magnometer.X = imu.Xmag;
                    uas.Magnometer.Y = imu.Ymag;
                    uas.Magnometer.Z = imu.Zmag;

                    break;
            }

        }
    }
}
