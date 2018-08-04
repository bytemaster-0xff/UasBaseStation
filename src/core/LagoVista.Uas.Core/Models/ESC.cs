using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class ESC : ModelBase
    {
        float _current;
        public float Current
        {
            get { return _current; }
            set { Set(ref _current, value); }
        }

        float _voltage;
        public float Voltage
        {
            get { return _voltage; }
            set { Set(ref _voltage, value); }
        }

        float _temperature;
        public float Temperature
        {
            get { return _temperature; }
            set { Set(ref _temperature, value); }
        }

        float _rpm;
        public float RPM
        {
            get { return _rpm; }
            set { Set(ref _rpm, value); }
        }
    }
}
