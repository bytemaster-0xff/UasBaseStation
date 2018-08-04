using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Vibration : GaugeBase
    {
        private float _x;
        public float X
        {
            get { return _x; }
            set { Set(ref _x, value); }
        }

        private float _y;
        public float Y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }

        private float _z;
        public float Z
        {
            get { return _z; }
            set { Set(ref _z, value); }
        }

        private uint _clip0;
        public uint Clipping0
        {
            get { return _clip0; }
            set { Set(ref _clip0, value); }
        }


        private uint _clip1;
        public uint Clipping1
        {
            get { return _clip1; }
            set { Set(ref _clip1, value); }
        }


        private uint _clip2;
        public uint Clipping2
        {
            get { return _clip2; }
            set { Set(ref _clip2, value); }
        }

        public void Update(UasVibration vibration)
        {
            Clipping0 = vibration.Clipping0;
            Clipping1 = vibration.Clipping1;
            Clipping2 = vibration.Clipping2;
            X = vibration.VibrationX;
            Y = vibration.VibrationY;
            Z = vibration.VibrationZ;
            TimeStamp = DateTime.Now;

            GaugeStatus = GaugeStatus.OK;
        }
        
    }
}
