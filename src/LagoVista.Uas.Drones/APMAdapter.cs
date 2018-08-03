using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Drones
{
    public class APMAdapter : IUasMessageAdapter
    {
        public void UpdateUas(IUas uas, UasMessage message)
        {
            var apm = uas as APM;

            switch(message.MessageId)
            {
                case Core.MavLink.UasMessages.Heartbeat:
                    break;
                case Core.MavLink.UasMessages.GpsRawInt:
                    break;

                case UasMessages.Attitude:
                    var att = message as UasAttitude;
                    uas.Pitch = att.Pitch;
                    uas.PitchSpeed = att.Pitchspeed;
                    uas.Roll = att.Roll;
                    uas.RollSpeed = att.Rollspeed;
                    uas.Yaw = att.Yaw;
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
