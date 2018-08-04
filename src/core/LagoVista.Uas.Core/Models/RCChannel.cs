using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public class RCChannel : ModelBase
    {
        public short _value;
        public short Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        public UInt16 _rawValue;
        public UInt16 RawValue
        {
            get { return _rawValue; }
            set { Set(ref _rawValue, value); }
        }
    }
}
