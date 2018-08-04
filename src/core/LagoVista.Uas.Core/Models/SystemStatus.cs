using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class SystemStatus : GaugeBase
    {

        private bool _boot;
        public bool Boot
        {
            get { return _boot; }
            set { Set(ref _boot, value); }
        }

        private bool _standby;
        public bool Standby
        {
            get { return _standby; }
            set { Set(ref _standby, value); }
        }

        private bool _unknown;
        public bool Unknown
        {
            get { return _unknown; }
            set { Set(ref _unknown, value); }
        }

        private bool _calilbrating;
        public bool Calibrating
        {
            get { return _calilbrating; }
            set { Set(ref _calilbrating, value); }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { Set(ref _active, value); }
        }


        private bool _critical;
        public bool Critical
        {
            get { return _critical; }
            set { Set(ref _critical, value); }
        }

        private bool _emergency;
        public bool Emergency
        {
            get { return _emergency; }
            set { Set(ref _critical, value); }
        }

        private bool _powerOff;
        public bool Poweroff
        {
            get { return _powerOff; }
            set { Set(ref _powerOff, value); }
        }

        private bool _flightTermintation;
        public bool FlightTermination
        {
            get { return _flightTermintation; }
            set { Set(ref _flightTermintation, value); }
        }
       
        public void Update(UasHeartbeat hb)
        {
            Standby = hb.SystemStatus == (byte)MavState.Standby;
            FlightTermination = hb.SystemStatus == (byte)MavState.FlightTermination;
            Unknown = hb.SystemStatus == (byte)MavState.Uninit;
            Calibrating = hb.SystemStatus == (byte)MavState.Calibrating;
            Critical = hb.SystemStatus == (byte)MavState.Critical;
            Emergency = hb.SystemStatus == (byte)MavState.Emergency;
            Poweroff = hb.SystemStatus == (byte)MavState.Poweroff;
            Active = hb.SystemStatus == (byte)MavState.Active;
            Boot = hb.SystemStatus == (byte)MavState.Boot;

            GaugeStatus = Emergency || Critical ? GaugeStatus.Error : GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }
    }
}
