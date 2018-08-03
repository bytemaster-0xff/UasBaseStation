using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Sensor : ModelBase
    {
        public Sensor(MavSysStatusSensor sensor, String name)
        {
            SysStatusSensor = sensor;
            Name = name;
        }

        public MavSysStatusSensor SysStatusSensor { get; private set; }

        public String Name { get; private set; }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set { Set(ref _enabled, value); }
        }

        private bool _healthy;
        public bool Healthy
        {
            get { return _healthy; }
            set { Set(ref _healthy, value); }
        }

        private bool _present;
        public bool Present
        {
            get { return _present; }
            set { Set(ref _present, value); }
        }

        DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { Set(ref _timeStamp, value); }
        }
    }
}
