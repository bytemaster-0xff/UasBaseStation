using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System.Linq;

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
                    uas.FlightController.Update(message as UasHeartbeat);
                    uas.SystemStatus.Update(message as UasHeartbeat);
                    break;
                case Core.MavLink.UasMessages.GpsRawInt:
                    uas.GPSs.First().Update(message as UasGpsRawInt);
                    uas.CurrentLocation = uas.GPSs.First().Location;
                    break;

                case UasMessages.SysStatus: uas.Sensors.Update(message as UasSysStatus); break;
                case UasMessages.AutopilotVersion: uas.FlightController.Update(message as UasAutopilotVersion); break;
                case UasMessages.Hwstatus: uas.FlightController.Update(message as UasHwstatus); break;
                case UasMessages.PowerStatus: uas.PowerStatus.Update(message as UasPowerStatus); break;                
                case UasMessages.Attitude: uas.Attitude.Update(message as UasAttitude); break;
                case UasMessages.BatteryStatus: uas.Batteries.First().Update(message as UasBatteryStatus); break;

                case UasMessages.MagCalProgress:
                    /* Compass Calibration Progress  */
                    break;
                case UasMessages.CompassmotStatus:
                    /* Compass Calibration Status  */
                    break;
                case UasMessages.EkfStatusReport: uas.EKFStatus.Update(message as UasEkfStatusReport); break;
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
                    uas.Acc.First().Update(message as UasRawImu, DOFSensorType.Accelerometer);
                    uas.Gyro.First().Update(message as UasRawImu, DOFSensorType.Gyro);
                    uas.Magnometer.First().Update(message as UasRawImu, DOFSensorType.Magnometer);
                    break;
            }

        }
    }
}
