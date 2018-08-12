using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class RangeFinder : GaugeBase
    {
        private float _distance;
        public float Distance
        {
            get { return _distance; }
            set { Set(ref _distance, value); }
        }

        private float _voltage;
        public float Voltage
        {
            get { return _voltage; }
            set { Set(ref _voltage, value); }
        }

        public void Update(UasRangefinder rangeFinder)
        {
            Distance = rangeFinder.Distance;
            Voltage = rangeFinder.Voltage;
            if(Distance > 0)
            {
                GaugeStatus = GaugeStatus.OK;
            }

            Debug.WriteLine("Distaince " + Distance);
        }

    }
}
