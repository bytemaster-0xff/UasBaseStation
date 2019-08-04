using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.FlightRecorder
{
    public class FlightEvent
    {
        public FlightEvent()
        {
            TimeStamp = DateTime.UtcNow.ToJSONString();
        }

        public string TimeStamp { get; set; }

        public string EventName { get; set; }
    }
}
