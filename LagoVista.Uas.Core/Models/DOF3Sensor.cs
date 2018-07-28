using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public class DOF3Sensor : ModelBase
    {
        private Int16 _x;
        public Int16 X
        {
            get { return _x; }
            set { Set(ref _x, value); }
        }

        private Int16 _y;
        public Int16 Y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }

        private Int16 _z;
        public Int16 Z
        {
            get { return _z; }
            set { Set(ref _z, value); }
        }
    }
}
