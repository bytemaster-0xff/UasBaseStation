using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class FlightController : ModelBase
    {
        float _boardVoltage;
        public float BoardVoltage
        {
            get { return _boardVoltage; }
            set { Set(ref _boardVoltage, value); }
        }

        int _i2cErrorCount;
        public int I2CErrorCount
        {
            get { return _i2cErrorCount; }
            set { Set(ref _i2cErrorCount, value); }
        }

        public Version Version { get; private set; }

        private DateTime _systemTime;
        public DateTime SystemType
        {
            get { return _systemTime; }
            set { Set(ref _systemTime, value); }
        }

        public void Update(UasHwstatus hwStatus)
        {
            I2CErrorCount = hwStatus.I2cerr;
            BoardVoltage = hwStatus.Vcc;
        }
    }
}
