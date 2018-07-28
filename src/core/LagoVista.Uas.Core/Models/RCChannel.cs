using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public class RCChannel : ModelBase
    {
        public UInt16 _value;
        public UInt16 Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }
    }
}
