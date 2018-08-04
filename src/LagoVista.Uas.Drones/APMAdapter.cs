using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Linq;
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
                    //TODO: Handle multiple GPS
                    var gpsMsg = message as UasGpsRawInt;
                    if(!uas.GPSs.Any())
                    {
                        uas.GPSs.Add(new GPS());
                    }

                    uas.GPSs.First().Location = new LagoVista.Core.Models.Geo.GeoLocation()
                    {
                        Altitude = gpsMsg.Alt,
                        Latitude = gpsMsg.Lat / 10000000.0f,
                        Longitude = gpsMsg.Lon / 10000000.0f
                    };

                    uas.CurrentLocation = uas.GPSs.First().Location;

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
                case UasMessages.RcChannels:
                    var rcRaw = message as UasRcChannels;
                    uas.Channels[0].RawValue = rcRaw.Chan1Raw;
                    uas.Channels[1].RawValue = rcRaw.Chan2Raw;
                    uas.Channels[2].RawValue = rcRaw.Chan3Raw;
                    uas.Channels[3].RawValue = rcRaw.Chan4Raw;
                    uas.Channels[4].RawValue = rcRaw.Chan5Raw;
                    uas.Channels[5].RawValue = rcRaw.Chan6Raw;
                    uas.Channels[6].RawValue = rcRaw.Chan7Raw;
                    uas.Channels[7].RawValue = rcRaw.Chan8Raw;

                    uas.Channels[8].RawValue = rcRaw.Chan9Raw;
                    uas.Channels[9].RawValue = rcRaw.Chan10Raw;
                    uas.Channels[10].RawValue = rcRaw.Chan11Raw;
                    uas.Channels[11].RawValue = rcRaw.Chan12Raw;
                    uas.Channels[12].RawValue = rcRaw.Chan13Raw;
                    uas.Channels[13].RawValue = rcRaw.Chan14Raw;
                    uas.Channels[14].RawValue = rcRaw.Chan15Raw;
                    uas.Channels[15].RawValue = rcRaw.Chan16Raw;
                    break;

                case UasMessages.RcChannelsScaled:
                    var rc = message as UasRcChannelsScaled;
                    uas.Channels[0].Value = rc.Chan1Scaled;
                    uas.Channels[1].Value = rc.Chan2Scaled;
                    uas.Channels[2].Value = rc.Chan3Scaled;
                    uas.Channels[3].Value = rc.Chan4Scaled;
                    uas.Channels[4].Value = rc.Chan5Scaled;
                    uas.Channels[5].Value = rc.Chan6Scaled;
                    uas.Channels[6].Value = rc.Chan7Scaled;
                    uas.Channels[7].Value = rc.Chan8Scaled;
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
