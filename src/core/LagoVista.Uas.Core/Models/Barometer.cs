using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Barometer : GaugeBase
    {
        float _rawPressure;
        public float RawPressure
        {
            get { return _rawPressure; }
            set { Set(ref _rawPressure, value); }
        }

        float _diffPressure;
        public float DifferentialPressure
        {
            get { return _diffPressure; }
            set { Set(ref _diffPressure, value); }
        }

        float _absPressure;
        public float AbsolutePressure
        {
            get { return _absPressure; }
            set { Set(ref _absPressure, value); }
        }

        short _temperature;
        public short Temperature
        {
            get { return _temperature; }
            set { Set(ref _temperature, value); }
        }

        float _rawTemperature;
        public float RawTemperature
        {
            get { return _rawTemperature; }
            set { Set(ref _rawTemperature, value); }
        }

        public void Update(UasSensorOffsets offsets)
        {
            RawPressure = offsets.RawPress;
            RawTemperature = offsets.RawTemp;
        }

        public void Update(UasScaledPressure pressure)
        {
            DifferentialPressure = pressure.PressAbs;
            Temperature = pressure.Temperature;
            GaugeStatus = GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }

        public void Update(UasScaledPressure2 pressure)
        {
            DifferentialPressure = pressure.PressAbs;
            Temperature = pressure.Temperature;
            GaugeStatus = GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }
    
        public void Update(UasScaledPressure3 pressure)
        {
            DifferentialPressure = pressure.PressAbs;
            Temperature = pressure.Temperature;
            GaugeStatus = GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }
    }
}
