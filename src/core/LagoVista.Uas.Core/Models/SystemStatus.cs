using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Utils;
using System;
using System.Linq;
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
            set { Set(ref _emergency, value); }
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

        private bool _armed;
        public bool Armed
        {
            get { return _armed; }
            set { Set(ref _armed, value); }
        }

        private bool _isFlying;
        public bool IsFlying
        {
            get { return _isFlying; }
            set { Set(ref _isFlying, value); }
        }

        private bool _stabilized;
        public bool Stabilized
        {
            get { return _stabilized; }
            set { Set(ref _stabilized, value); }
        }


        private bool _manual;
        public bool Manual
        {
            get { return _manual; }
            set { Set(ref _manual, value); }
        }

        private bool _hardwareInLoopSimulation;
        public bool HardwareInLoopSimulation
        {
            get { return _hardwareInLoopSimulation; }
            set { Set(ref _hardwareInLoopSimulation, value); }
        }

        private bool _guided;
        public bool Guided
        {
            get { return _guided; }
            set { Set(ref _guided, value); }
        }

        private bool _auto;
        public bool Autonomous
        {
            get { return _auto; }
            set { Set(ref _auto, value); }
        }

        private bool _test;
        public bool Test
        {
            get { return _test; }
            set { Set(ref _test, value); }
        }

        private bool _compassNeedsCalibration = false;
        public bool CompassNeedsCaliabration
        {
            get { return _compassNeedsCalibration; }
            set { Set(ref _compassNeedsCalibration, value); }
        }

        private TimeSpan _flightTime = TimeSpan.Zero;
        public TimeSpan FlightTime
        {
            get { return _flightTime; }
            set { Set(ref _flightTime, value); }
        }

        private bool _lowBatteryWarning;
        public bool LowBatteryWarning
        {
            get { return _lowBatteryWarning; }
            set { Set(ref _lowBatteryWarning, value); }
        }

        private bool _criticalBatteryWarning;
        public bool CriticalBatteryWarning
        {
            get { return _criticalBatteryWarning; }
            set { Set(ref _criticalBatteryWarning, value); }
        }

        private string _customMode = String.Empty;
        public string CustomMode
        {
            get { return _customMode; }
            set { Set(ref _customMode, value); }
        }

        private uint _customModeType;

        MavAutopilot _autoPilot;
        public MavAutopilot Autopilot
        {
            get { return _autoPilot; }
            set
            {
                Set(ref _autoPilot, value);
                switch (value)
                {
                    
                }
            }
        }

        private MavType _mavType;
        public MavType Type
        {
            get { return _mavType; }
            set { Set(ref _mavType, value); }
        }

        private Firmwares _firmware;
        public Firmwares Firmware
        {
            get { return _firmware; }
            set { Set(ref _firmware, value); }
        }
    
        public void Update(UasHeartbeat hb)
        {
            Autopilot = (MavAutopilot)hb.Autopilot;
            Type = (MavType)hb.Type;

            switch(Autopilot)
            {
                case MavAutopilot.Ardupilotmega:
                    switch (Type)
                    {
                        case MavType.FixedWing:
                            Firmware = Firmwares.ArduPlane;
                            break;
                        case MavType.Quadrotor:
                        case MavType.Coaxial:
                        case MavType.Helicopter:
                        case MavType.Octorotor:
                        case MavType.Tricopter:
                        case MavType.Hexarotor:
                            Firmware = Firmwares.ArduCopter2;

                            break;
                        case MavType.AntennaTracker:
                            Firmware = Firmwares.ArduTracker;
                            break;
                        case MavType.GroundRover:
                        case MavType.SurfaceBoat:
                            Firmware = Firmwares.ArduRover;
                            break;
                        case MavType.Submarine:
                            Firmware = Firmwares.ArduSub;
                            break;
                        default:
                            Firmware = Firmwares.Unknown;
                            break;
                    }
                    break;
                case MavAutopilot.Px4:
                    Firmware = Firmwares.PX4;
                    break;
                case MavAutopilot.Generic:
                case MavAutopilot.Reserved:
                case MavAutopilot.Slugs:
                case MavAutopilot.Openpilot:
                case MavAutopilot.GenericWaypointsOnly:
                case MavAutopilot.GenericWaypointsAndSimpleNavigationOnly:
                case MavAutopilot.GenericMissionFull:
                case MavAutopilot.Invalid:
                case MavAutopilot.Ppz:
                case MavAutopilot.Udb:
                case MavAutopilot.Fp:
                case MavAutopilot.Smaccmpilot:
                case MavAutopilot.Autoquad:
                case MavAutopilot.Armazila:
                case MavAutopilot.Aerob:
                case MavAutopilot.Asluav:
                case MavAutopilot.Smartap:
                case MavAutopilot.Airrails:
                    Firmware = Firmwares.Unknown;
                    break;
            }

            Armed = (hb.BaseMode & (byte)MavModeFlag.SafetyArmed) == (byte)MavModeFlag.SafetyArmed;
            Guided = (hb.BaseMode & (byte)MavModeFlag.GuidedEnabled) == (byte)MavModeFlag.GuidedEnabled;
            HardwareInLoopSimulation = (hb.BaseMode & (byte)MavModeFlag.HilEnabled) == (byte)MavModeFlag.HilEnabled;
            Autonomous = (hb.BaseMode & (byte)MavModeFlag.AutoEnabled) == (byte)MavModeFlag.AutoEnabled;
            Stabilized = (hb.BaseMode & (byte)MavModeFlag.StabilizeEnabled) == (byte)MavModeFlag.StabilizeEnabled;
            Manual = (hb.BaseMode & (byte)MavModeFlag.ManualInputEnabled) == (byte)MavModeFlag.ManualInputEnabled;
            var customModeEnabled = (hb.BaseMode & (byte)MavModeFlag.CustomModeEnabled) == (byte)MavModeFlag.CustomModeEnabled;

            if (customModeEnabled)
            {
                if (_customModeType != hb.CustomMode)
                {
                    _customModeType = hb.CustomMode;
                    var modelist = Modes.getModesList(Firmware);
                    if (modelist != null)
                    {
                        var customMode = modelist.Where(mod => mod.Key == hb.CustomMode).FirstOrDefault();
                        if (customMode.Key == hb.CustomMode)
                        {
                            CustomMode = customMode.Value;
                        }
                        else
                        {
                            CustomMode = String.Empty;
                        }
                    }
                }
            }
            else
            {
                CustomMode = String.Empty;
            }
          
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
