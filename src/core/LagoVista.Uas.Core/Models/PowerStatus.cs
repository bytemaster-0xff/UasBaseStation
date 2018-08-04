using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class PowerStatus : GaugeBase
    {
        private ushort _vcc;
        public ushort VCC
        {
            get { return _vcc; }
            set { Set(ref _vcc, value); }
        }

        private ushort _vServo;
        public ushort VServo
        {
            get { return _vServo; }
            set { Set(ref _vServo, value); }
        }

        private uint _flags;
        public uint Flags
        {
            get { return _flags; }
        }

        public void Update(UasPowerStatus powerStatus)
        {
            VCC = powerStatus.Vcc;
            VServo = powerStatus.Vservo;

            TimeStamp = DateTime.Now;
            GaugeStatus = GaugeStatus.OK;
        }

    }
}
