using LagoVista.Core.Models;
using LagoVista.Core.Models.Drawing;
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
            set {
                Set(ref _enabled, value);
                if (Present)
                {
                    if (Enabled && Healthy)
                    {
                        BGColor = NamedColors.Green;
                        FGColor = NamedColors.White;
                    }
                    else if (Present)
                    {
                        BGColor = NamedColors.Yellow;
                        FGColor = NamedColors.Black;

                    }
                    else if (!Healthy)
                    {
                        BGColor = NamedColors.Red;
                        FGColor = NamedColors.White;
                    }
                }
            }
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


        Color _bgColor = NamedColors.White;
        public Color BGColor
        {
            get { return _bgColor; }
            set { Set(ref _bgColor, value); }
        }

        Color _fgColor = NamedColors.Black;
        public Color FGColor
        {
            get { return _fgColor; }
            set { Set(ref _fgColor, value); }
        }
    }
}
