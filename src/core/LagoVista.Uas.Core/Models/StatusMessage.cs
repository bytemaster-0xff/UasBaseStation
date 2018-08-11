using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class StatusMessage
    {
        public StatusMessage(String message)
        {
            TimeStamp = DateTime.Now;
            Message = message;
        }

        public DateTime TimeStamp { get; }
        public string Message { get; }

        public bool IsExpired
        {
            get { return TimeStamp < DateTime.Now - TimeSpan.FromSeconds(10); }
        }

        public override string ToString()
        {
            return $"{TimeStamp} - {Message}";
        }

        public string Display
        {
            get { return $"{TimeStamp} - {Message}"; }
        }
    }
}
