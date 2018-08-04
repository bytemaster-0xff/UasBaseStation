using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public enum GaugeStatus
    {
        Unknown,
        OK,
        Error,
        Warning
    }



    public class GaugeBase : ModelBase
    {
        public GaugeBase()
        {
            Errors = new List<Error>();
        }


        private List<Error> _errors;
        public List<Error> Errors
        {
            get { return _errors; }
            set { Set(ref _errors, value); }
        }

        private GaugeStatus _status = GaugeStatus.Unknown;
        public GaugeStatus GaugeStatus
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        private DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { Set(ref _timeStamp, value); }
        }
    }
}
