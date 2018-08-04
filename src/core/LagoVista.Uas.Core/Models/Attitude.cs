using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Attitude : GaugeBase
    {

        public float _pitch;
        public float Pitch
        {
            get => _pitch;
            set => Set(ref _pitch, value);
        }

        public float _yaw;
        public float Yaw
        {
            get => _yaw;
            set => Set(ref _yaw, value);
        }

        public float _roll;
        public float Roll
        {
            get => _roll;
            set => Set(ref _roll, value);
        }

        public float _pitchSpeed;
        public float PitchSpeed
        {
            get => _pitchSpeed;
            set => Set(ref _pitchSpeed, value);
        }

        public float _yawSpeed;
        public float YawSpeed
        {
            get => _yawSpeed;
            set => Set(ref _yawSpeed, value);
        }

        public float _rollSpeed;
        public float RollSpeed
        {
            get => _rollSpeed;
            set => Set(ref _rollSpeed, value);
        }

        public void Update(UasAttitude attitude)
        {
            RollSpeed = attitude.Rollspeed;
            YawSpeed = attitude.Yawspeed;
            PitchSpeed = attitude.Pitchspeed;
            Roll = attitude.Roll.ToDegrees();
            var yaw = attitude.Yaw.ToDegrees();
            Yaw = (yaw < 0) ?  360.0f + yaw : yaw ;
            Pitch = attitude.Pitch.ToDegrees();

            TimeStamp = DateTime.Now;
            GaugeStatus = GaugeStatus.OK;
        }
    }
}
